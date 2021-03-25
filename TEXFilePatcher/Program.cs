﻿using System;
using System.IO;
using System.Linq;

namespace TEXFilePatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Please provide a path to the input file.");
            }

            if (args[0].Contains("_patch"))
            {
                throw new Exception("Already patched.");
            }

            using (FileStream fstream = new FileStream(args[0], FileMode.Open))
            {
                Console.WriteLine("Reading input stream from " + args[0]);

                // Start reading from 12th byte
                fstream.Seek(32, SeekOrigin.Begin);

                // Code from MS Docs :)
                byte[] input = new byte[fstream.Length];
                int numBytesToRead = (int)fstream.Length - 12;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fstream.Read(input, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                numBytesToRead = input.Length;

                // Write the byte array to the other FileStream.
                using (FileStream fsNew = new FileStream(Path.GetFileNameWithoutExtension(args[0]) + "_patch.dds",
                    FileMode.Create, FileAccess.Write))
                {
                    // Write to the output file
                    Console.WriteLine("Writing output...");
                    fsNew.Write(input, 0, numBytesToRead);
                }
            }
            Console.WriteLine("Done.");
        }
    }
}

