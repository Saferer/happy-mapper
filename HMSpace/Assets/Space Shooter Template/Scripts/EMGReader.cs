using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;


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
        private double max = 0;
        private double runningAverage = 0;
        SerialPort serialPort;
        private bool keepRunning = true;

        //Constructor
        public EMGReader()
        {

            //initializeProgram();
            serialPort = new SerialPort("/dev/cu.usbmodem14201", 57600, Parity.None);
            if (serialPort.IsOpen){
              serialPort.Close();
            }
            serialPort.Open();
            packetBytes = new int[17];
            pastValues = new double[52];
        }

        //Run this code on a serperate thread. This already loops so do not need to run this in loop
        public void run()
        {
            while(keepRunning)
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
                        double[] channels = new double[6];
                        double average = 0;
                        for (int i = 0; i < channels.Length; i+=2)
                        {
                            channels[i] = transform((int)(packetBytes[i+4] << 8 | packetBytes[i+5]));
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

                        if (pastIndex >= pastValues.Length)
                        {
                            pastIndex = 0;
                        }
                        Console.WriteLine(getPercentage()); //comment this out later
                        index = 2;
                        break;
                    }
                    packetBytes[index++] = serialPort.ReadByte();

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

        //Get measured ratio of current window compared to max
        public double getPercentage()
        {
            return (double)runningAverage / max;
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

        //Starts Calibration for measuring maximum
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

        }

        //Set the max value
        public void setMax(double max)
        {
            this.max = max;
        }

        public void setFlag(bool val){
          this.keepRunning = val;
        }

        public void close(){
          serialPort.Close();
        }

        //Main to test code
        public static void Main(string[] args)
        {
            EMGReader test = new EMGReader();

            Console.ReadLine();
            //test.calibrateBase();
            Console.WriteLine("Done");
            Console.ReadLine();
            test.calibrateMax();
            Console.WriteLine("Done");
            Console.ReadLine();
            test.run();
        }
    }
}
