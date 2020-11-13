using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System.IO;
using System.Threading;
using SYSTEM = System.Diagnostics;



namespace I4HUSB
{
    public class EMGReader
    {
        private int[] packetBytes;
        private double[] pastValues;
        private double deletedValue = 0;
        private int pastIndex = 0;
        private double basepoint = 300;
        private int index = 2;
        private volatile float goal = 0;
        private double runningAverage = 0;
        SerialPort serialPort = null;
        private bool keepRunning = true;
        private double AVERAGE_PERIOD = 0.2;  // seconds
        private double RATE = 252f; // hz
        private double CALIBRATION_TIME = 5; //seconds
        private bool debugMode = false;
        private bool record = false;
        private List<float> recordedValues;
        private long timeToRecord;
        private SYSTEM.Stopwatch stopwatch;
        private Mutex mut;
        private Mutex mut2;

        private BoolWrapper signal;
        //Constructor


        private float[] calibrationResults;
        public EMGReader(bool debug = false)
        {
            debugMode = debug;
            //initializeProgram();
            var portNames = SerialPort.GetPortNames();
            Debug.Log(portNames.Length);
            foreach (var name in portNames)
            {
                Debug.Log(name);
            }
            if (!debug)
            {
                // serialPort = new SerialPort("COM6", 57600, Parity.None);
                // if (!serialPort.IsOpen)
                // {
                //     serialPort.Open();
                // }
            }


            packetBytes = new int[17];

            int size = (int)Math.Round(RATE * AVERAGE_PERIOD);
            pastValues = new double[size];
            mut = new Mutex();
            mut2 = new Mutex();
            recordedValues = new List<float>();
            stopwatch = new SYSTEM.Stopwatch();

        }

        //Run this code on a serperate thread. This already loops so do not need to run this in loop
        public void run()
        {
            if(serialPort == null)
            {
                Debug.Log("No port has been set");
                return;
            }
            if (!debugMode)
            {
                if(!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
                while (keepRunning)
                {
                    while (keepRunning)
                    {
                        packetBytes[0] = serialPort.ReadByte();
                        if (packetBytes[0] == 0xa5)
                        {
                            //Console.WriteLine("Found a5");
                            packetBytes[1] = serialPort.ReadByte();
                            if (packetBytes[1] == 0x5a)
                            {
                                //Console.WriteLine("Found 5a");
                                break;
                            }
                        }
                    }
                    while (keepRunning)
                    {

                        if (index > 16)
                        {
                            double[] channels = new double[6];
                            double average = 0;
                            for (int i = 0; i < channels.Length; i += 2)
                            {
                                channels[i] = transform((int)(packetBytes[i + 4] << 8 | packetBytes[i + 5]));
                                average += channels[i];
                            }

                            /*
                            Console.WriteLine(packetBytes[4] << 8 | packetBytes[5]);
                            Console.WriteLine(packetBytes[6] << 8 | packetBytes[7]);
                            Console.WriteLine(packetBytes[8] << 8 | packetBytes[9]);
                            */
                            average /= channels.Length;
                            //max = average > max ? average : max;
                            deletedValue = pastValues[pastIndex];

                            pastValues[pastIndex++] = average;
                            calculateRunningAverage(average);
                            if (record)
                            {
                                mut.WaitOne();
                                recordedValues.Add((float)runningAverage);
                                mut.ReleaseMutex();
                                if (stopwatch.ElapsedMilliseconds > timeToRecord)
                                {
                                    stopwatch.Stop();
                                    record = false;
                                    signal.value = false;
                                }
                            }
                            if (pastIndex >= pastValues.Length)
                            {
                                pastIndex = 0;
                            }
                            //Console.WriteLine(getPercentage()); //comment this out later
                            index = 2;
                            break;
                        }
                        packetBytes[index++] = serialPort.ReadByte();
                    }
                }
            }
            //Debug.Log(getPercentage());
        }

        //Transform values with zero reference
        private double transform(double channel)
        {
            channel -= basepoint;
            channel = Math.Abs(channel);
            return channel;
        }

        //Calculates window
        private void calculateRunningAverage(double newValue)
        {
            runningAverage += (newValue / pastValues.Length);
            runningAverage -= (deletedValue / pastValues.Length);
        }

        //Get measured ratio of current window compared to max with a range of (0,2)
        public double getPercentage()
        {
            mut2.WaitOne();
            double result = (double)runningAverage / goal;
            if (result > 2) { result = 2; }
            Debug.Log("EMGReader: GetPercentage: " + runningAverage + ":" + goal);
            mut2.ReleaseMutex();
            return result;
        }

        //Starts Calibration for zero point (When measuring base do not move the muscle)
        public void calibrateBase()
        {
            serialPort.DiscardInBuffer();
            double basepointAverage = 0;
            for (int i = 0; i < 1000; i++)
            {
                while (true)
                {
                    packetBytes[0] = serialPort.ReadByte();
                    if (packetBytes[0] == 0xa5)
                    {
                        //Console.WriteLine("Found a5");
                        packetBytes[1] = serialPort.ReadByte();
                        if (packetBytes[1] == 0x5a)
                        {
                            //Console.WriteLine("Found 5a");
                            break;
                        }

                    }
                }
                while (true)
                {

                    if (index > 16)
                    {
                        double channel1 = (int)(packetBytes[4] << 8 | packetBytes[5]);
                        double channel2 = (int)(packetBytes[6] << 8 | packetBytes[7]);
                        double channel3 = (int)(packetBytes[8] << 8 | packetBytes[9]);
                        /*
                        Console.WriteLine(packetBytes[4] << 8 | packetBytes[5]);
                        Console.WriteLine(packetBytes[6] << 8 | packetBytes[7]);
                        Console.WriteLine(packetBytes[8] << 8 | packetBytes[9]);
                        */
                        basepointAverage += (channel1 + channel2 + channel3) / (double)3;
                        //max = average > max ? average : max;
                        index = 0;
                        break;
                    }
                    packetBytes[index++] = serialPort.ReadByte();

                }
            }

            basepointAverage /= 1000;
            basepoint = basepointAverage;
            //Console.WriteLine(basepoint);

        }

        public void StartRecord(int time, BoolWrapper signal)
        {
            Debug.Log("EMGReader: StartRecord: Starting Function");
            record = true;
            recordedValues = new List<float>();
            this.signal = signal;
            timeToRecord = time * 1000;
            stopwatch.Reset();
            stopwatch.Start();
            int size = (int)Math.Round(RATE * AVERAGE_PERIOD);
            pastValues = new double[size];
            Debug.Log("EMGReader: StartRecord: Leaving Function");
        }

        public List<float> GetRecordedValues()
        {
            mut.WaitOne();
            List<float> copy = new List<float>(recordedValues);
            mut.ReleaseMutex();
            return copy;
        }

        public void setFlag(bool val)
        {
            this.keepRunning = val;
        }

        public void setGoal(float goal)
        {
            mut2.WaitOne();
            this.goal = goal;
            Debug.Log("EMGReader: SetGoal: Goal is " + this.goal);
            mut2.ReleaseMutex();
        }
        
        public double getGoal()
        {
            mut2.WaitOne();
            float result = this.goal;
            mut2.ReleaseMutex();
            if(this.goal == 0)
            {
                Debug.Log("EMGReader: GetGoal: Goal is still zero");
            }
            return result;
        }
        
        public void close()
        {
            if (serialPort != null)
            {
                serialPort.Close();
            }

        }

        public double RunningAverage
        {
            get { return runningAverage; }
            set
            {
                if (debugMode)
                {
                    runningAverage = value;
                }
            }
        }

        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public void SetPort(string name)
        {
            if(serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
            Debug.Log("EMGReader: SetPort: Setting port to: " + name);
            serialPort = new SerialPort(name, 57600, Parity.None);
        }
        //Main to test code
        public static void Main(string[] args)
        {
            EMGReader test = new EMGReader();

            Console.ReadLine();
            //test.calibrateBase();
            Console.WriteLine("Done");
            Console.ReadLine();
            // test.calibrateMax();
            Console.WriteLine("Done");
            Console.ReadLine();
            test.run();
        }

        public void TestThis(EMGReader test)
        {
            if(test == this)
            {
                Debug.Log("EMGReader: TestThis: True!");
            }
            else
            {
                Debug.Log("EMGReader: TestThis: False!");
            }
        }
    }

            //**DEPRECATED** -- Use getCalibrationArray() and then setGoal
        //Starts Calibration for measuring maximum
        /*
        public void calibrateMax()
        {
            serialPort.DiscardInBuffer();
            double[] runningArray = new double[26];
            int currentIndex = 0;
            for (int i = 0; i < 800; i++)
            {
                while (true)
                {
                    //Console.WriteLine(i);
                    packetBytes[0] = serialPort.ReadByte();
                    if (packetBytes[0] == 0xa5)
                    {
                        //Console.WriteLine("Found a5");
                        packetBytes[1] = serialPort.ReadByte();
                        if (packetBytes[1] == 0x5a)
                        {
                            //Console.WriteLine("Found 5a");
                            break;
                        }

                    }
                }
                while (true)
                {

                    if (index > 16)
                    {

                        double[] channels = new double[6];
                        double average = 0;
                        for (int j = 0; j < channels.Length; j += 2)
                        {
                            channels[j] = transform((int)(packetBytes[j + 4] << 8 | packetBytes[j + 5]));
                            average += channels[j];

                        }


                        runningArray[currentIndex++] = (double)average/channels.Length;
                        if (currentIndex >= 26)
                        {
                            currentIndex = 0;
                        }
                        double runningAverageMax = 0;
                        foreach(double k in runningArray)
                        {
                            runningAverageMax += k;
                        }
                        max = (runningAverageMax / runningArray.Length) > max ? (runningAverageMax / runningArray.Length) : max;
                        //max = average > max ? average : max;
                        index = 2;
                        break;
                    }

                    packetBytes[index++] = serialPort.ReadByte();



                }
            }

            Debug.Log(max);
            //Console.WriteLine(max);

        }*/

        //**ALSO DECAPRECATED CALIBRATION SHALL BE HANDLED BY GAME CLIETN INSTEAD
        //Use instead of calibrateMax to get data for the next CALIBRATION_TIME seconds
        // public double[] getCalibrationArray()
        // {
        //     serialPort.DiscardInBuffer();
        //     int rounded = (int)Math.Round(CALIBRATION_TIME * RATE);
        //     double[] runningArray = new double[rounded];
        //     int currentIndex = 0;
        //     for (int i = 0; i < CALIBRATION_TIME * RATE; i++)
        //     {
        //         while (true)
        //         {
        //             //Console.WriteLine(i);
        //             packetBytes[0] = serialPort.ReadByte();
        //             if (packetBytes[0] == 0xa5)
        //             {
        //                 //Console.WriteLine("Found a5");
        //                 packetBytes[1] = serialPort.ReadByte();
        //                 if (packetBytes[1] == 0x5a)
        //                 {
        //                     //Console.WriteLine("Found 5a");
        //                     break;
        //                 }

        //             }
        //         }
        //         while (true)
        //         {

        //             if (index > 16)
        //             {
        //                 double[] channels = new double[6];
        //                 double average = 0;
        //                 for (int j = 0; j < channels.Length; j += 2)
        //                 {
        //                     channels[j] = transform((int)(packetBytes[j + 4] << 8 | packetBytes[j + 5]));
        //                     average += channels[j];
        //                 }

        //                 /*
        //                 Console.WriteLine(packetBytes[4] << 8 | packetBytes[5]);
        //                 Console.WriteLine(packetBytes[6] << 8 | packetBytes[7]);
        //                 Console.WriteLine(packetBytes[8] << 8 | packetBytes[9]);
        //                 */
        //                 average /= channels.Length;
        //                 //max = average > max ? average : max;
        //                 deletedValue = pastValues[pastIndex];

        //                 pastValues[pastIndex++] = average;
        //                 calculateRunningAverage(average);

        //                 if (pastIndex >= pastValues.Length)
        //                 {
        //                     pastIndex = 0;
        //                 }

        //                 runningArray[currentIndex++] = this.runningAverage;

        //                 index = 2;
        //                 break;
        //             }
        //             packetBytes[index++] = serialPort.ReadByte();



        //         }
        //     }

        //     return runningArray;

        // }



        //Set the max value
        /*
        public void setMax(double max)
        {
            this.max = max;
        }*/
}
