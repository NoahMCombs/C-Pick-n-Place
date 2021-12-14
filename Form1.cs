using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.CvEnum;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Lab6_7
{
    public partial class Form1 : Form
    {
        //vectors for coords to be passed to arduino
        int xInput = 0;
        int yInput = 0;
        int typeToSend = 0;


        //Initialize serial communication wit arduino
        SerialPort arduinoSerial = new SerialPort();
        byte[] buffer = new byte[3];
        bool pointBufferEmpty = true;
        bool enableCoordinateSending = true;

        //initialize form
        public Form1()
        {
            InitializeComponent();
        }

        //initialize video capture and capture thread
        VideoCapture _capture;
        Thread _captureThread;
        Thread serialMonitoringThread;

        //Load form, set up video capture and capture thread. Set up serial communication with arduino, clear buffer 
        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                arduinoSerial.PortName = "COM10";
                arduinoSerial.BaudRate = 9600;
                arduinoSerial.Open();
                arduinoSerial.DiscardOutBuffer();
                arduinoSerial.DiscardInBuffer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error initializing COM port");
                Close();
            }

            _capture = new VideoCapture(0);
            _captureThread = new Thread(ProcessImage);
            _captureThread.Start();
            serialMonitoringThread = new Thread(MonitorSerialData);
            serialMonitoringThread.Start();
        }


        //Passes coordinates of shape to be grabbed in terms of half inches
        private void CalcCoord(Point shapePoint, int type, Size boxSize)
        {
            double xval = shapePoint.X;
            double yval = shapePoint.Y;

            double vert, hor;
            int horPos, vertPos;
            
            //convert coords. 22 x 17 grid
            hor = xval / boxSize.Width * 11 * 2;
            vert = (yval / boxSize.Height) * 8.5 * 2;
            hor = Math.Round(hor);
            vert = Math.Round(vert);

            horPos = Convert.ToInt32(hor);
            vertPos = Convert.ToInt32(vert);

            xInput = horPos;
            yInput = vertPos;
            typeToSend = type;

            byte[] buffer = new byte[5] { Encoding.ASCII.GetBytes("<")[0], Convert.ToByte(xInput), Convert.ToByte(yInput), Convert.ToByte(typeToSend), Encoding.ASCII.GetBytes(">")[0] };
            arduinoSerial.Write(buffer, 0, 5);

        }


        //Video processing
        private void ProcessImage()
        {
            while(_capture.IsOpened)
            {
                var workingImage = _capture.QueryFrame();

                // resize to PictureBox aspect ratio
                int newHeight = (workingImage.Size.Height * sourcePictureBox.Size.Width) / workingImage.Size.Width;
                Size newSize = new Size(sourcePictureBox.Size.Width, newHeight);
                CvInvoke.Resize(workingImage, workingImage, newSize);

                // as a test for comparison, create a copy of the image with a binary filter:
                var binaryImage = workingImage.ToImage<Gray, byte>().ThresholdBinary(new Gray(125), new
                Gray(255)).Mat;

                // Sample for gaussian blur:
                var blurredImage = new Mat();
                var cannyImage = new Mat();
                var decoratedImage = new Mat();
                CvInvoke.GaussianBlur(workingImage, blurredImage, new Size(5, 5), 0);

                // convert to B/W
                CvInvoke.CvtColor(blurredImage, blurredImage, typeof(Bgr), typeof(Gray));

                // apply canny:
                // NOTE: Canny function can frequently create duplicate lines on the same shape
                //       depending on blur amount and threshold values, some tweaking might be needed.
                //       You might also find that not using Canny and instead using FindContours on
                //       a binary-threshold image is more accurate.
                CvInvoke.Canny(blurredImage, cannyImage, 100, 255);

                // make a copy of the canny image, convert it to color for decorating:
                CvInvoke.CvtColor(cannyImage, decoratedImage, typeof(Gray), typeof(Bgr));

                // find contours:
                using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                using (VectorOfPoint approxContour = new VectorOfPoint())
                using (VectorOfPoint circlePoints = new VectorOfPoint())
                {
                    // Build list of contours
                    CvInvoke.FindContours(cannyImage, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                    int shapeCount = 0;

                    //Iterate through each contour
                    for (int i = 0; i < contours.Size; i++)
                    {
                        //Make polygon
                        VectorOfPoint contour = contours[i];
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        
                        //If the polygon has large area, determine triangle or square. Find center point. Draw Trianges blue, squares yellow. Draw centerpoints as green
                        if (CvInvoke.ContourArea(contour, false) > 250)
                        {
                            //Variable for centerpoint of polygon, and array of points of polygon. 
                            Point circlep = new Point(0,0);
                            Point[] points = approxContour.ToArray();
                            
                            //If polygon is a triangle
                            if (approxContour.Size == 3)
                            {
                                //Draw contour as blue
                                CvInvoke.Polylines(decoratedImage, approxContour, true, new Bgr(Color.Blue).MCvScalar);

                                //Find centerpoint for polygon
                                for (int j = 0; j < points.Length; j++)
                                {
                                    circlep.X += points[j].X;
                                    circlep.Y += points[j].Y;
                                }
                                circlep.X /= 3;
                                circlep.Y /= 3;

                                //Draw circle on center point for polygon
                                CvInvoke.Circle(decoratedImage, circlep, 1, new Bgr(Color.Green).MCvScalar, 4);
                                shapeCount++;
                                //IF coordsending is enabled, send coords
                                if(enableCoordinateSending)
                                    CalcCoord(circlep, 3, workingImage.Size);
                            }
                            //If polygon is a square
                            else if (approxContour.Size == 4)
                            {
                                //Draw contour as yellow
                                CvInvoke.Polylines(decoratedImage, approxContour, true, new Bgr(Color.Yellow).MCvScalar);

                                //Find contour center point
                                for (int j = 0; j < points.Length; j++)
                                {
                                    circlep.X += points[j].X;
                                    circlep.Y += points[j].Y;
                                }
                                circlep.X /= 4;
                                circlep.Y /= 4;

                                //Draw center point as circle. 
                                CvInvoke.Circle(decoratedImage, circlep, 1, new Bgr(Color.Green).MCvScalar, 4);
                                shapeCount++;

                                //if coord sending is enabled, send coords
                                if (enableCoordinateSending)
                                    CalcCoord(circlep, 3, workingImage.Size);

                            }
                            else
                            {
                                CvInvoke.Polylines(decoratedImage, approxContour, true, new Bgr(Color.Red).MCvScalar);
                            }

                        }
                    }
                    //display shape count and countour count
                    Invoke(new Action(() =>
                    {
                    shapeCountLbl.Text = $"There are {shapeCount} shapes detected";
                    }));

                    Invoke(new Action(() =>
                    {
                    contourLbl.Text = $"There are {contours.Size} contours detected";
                    }));
                    }
                // output images:
                sourcePictureBox.Image = workingImage.Bitmap;
                contourPictureBox.Image = decoratedImage.Bitmap;
            }
        }


        private void MonitorSerialData()
        {
            while (true)
            { 

                // block until \n character is received, extract command data
                string msg = arduinoSerial.ReadLine();
                /*
                Invoke(new Action(() =>
                {
                    returnedPointLbl.Text = $"Returned Point Data: {msg}";
                }));
                */
                // confirm the string has both < and > characters
                if (msg.IndexOf("<") == -1 || msg.IndexOf(">") == -1)
                {
                    continue;
                }
                // remove everything before (and including) the < character
                msg = msg.Substring(msg.IndexOf("<") + 1);
                // remove everything after (and including) the > character
                msg = msg.Remove(msg.IndexOf(">"));
                // if the resulting string is empty, disregard and move on
                if (msg.Length == 0)
                {
                    continue;
                }
                // parse the command
                if (msg.Substring(0, 1) == "S")
                {
                    // command is to suspend, toggle states accordingly:
                    ToggleFieldAvailability(msg.Substring(1, 1) == "1");
                }
                else if (msg.Substring(0, 1) == "P")
                {
                    xInput = 0;
                    yInput = 0;
                    typeToSend = 0;
                    pointBufferEmpty = true;
                    // command is to display the point data, output to the text field:
                    Invoke(new Action(() =>
                    {
                        returnedPointLbl.Text = $"Returned Point Data: {msg.Substring(2)}";
                    }));
                }
            }
        }

        private void ToggleFieldAvailability(bool suspend)
        {
            enableCoordinateSending = !suspend;   
        }


        //Abort capture thread when form closes
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _captureThread.Abort();
            serialMonitoringThread.Abort();
        }
    }
}
