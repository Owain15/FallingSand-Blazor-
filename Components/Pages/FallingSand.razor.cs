using FallingSand.Components.Layout;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;

namespace FallingSand.Components.Pages
{

    public partial class FallingSand
    {

        int GridSizeX = 60;
        int GridSizeY = 60;

        System.Timers.Timer timer;
        int RefreshRate = 1000;

        List<(int,int)> SandList = new List<(int,int)>();
		bool[,] gameData;
        Pixel[,] displayData;

        public int intervilCount = 50;
        public string testClockReadOut;

        protected override void OnInitialized()
        {
            testClockReadOut = intervilCount.ToString();
            
            //timer = new System.Timers.Timer();

            SandList.Clear();

            gameData = new bool[GridSizeX,GridSizeY];
          
            displayData = new Pixel[GridSizeX,GridSizeY];

            StartTimmer();

		    InitialiseDisplay();
           
            //test sand. Remove.
            gameData[10, 10] = true;
			gameData[10, 11] = true;
			gameData[11, 10] = true;
			gameData[11, 11] = true;

		
			displayData = Display.GetPixelDataFromGameState(gameData);
		}


		void StartTimmer()
        {

            timer = new System.Timers.Timer(RefreshRate);
			timer.Elapsed += HandelTimeElapsedEvent;
			timer.AutoReset = true;
            timer.Enabled = true;
            
        }

      
        public void HandelTimeElapsedEvent(Object source , System.Timers.ElapsedEventArgs e)
        {
		
			intervilCount++;
            
            Console.WriteLine($"intervelCount = {intervilCount}");
            
            //base.ShouldRender();
            //StateHasChanged();
        }
        private void SetClockRender() 
        {
            Console.WriteLine("testClockReadOut Updated");
            testClockReadOut = intervilCount.ToString();
        }

		void InitialiseDisplay()
		{

			for (int y = 0; y < GridSizeY; y++)
			{
				for (int x = 0; x < GridSizeX; x++)
				{
					displayData[x, y] = new Pixel(x, y);

					//Randomize Color 
					Random ran = new Random();
					displayData[x, y].Color.Red = ran.Next(0, 250);
					displayData[x, y].Color.Blue = ran.Next(0, 250);
					displayData[x, y].Color.Green = ran.Next(0, 250);


				}
			}

		}

   
	}

    
    public static class Display 
    {

        public static Pixel[,] GetPixelDataFromGameState(bool[,] gameData)
        {
            Pixel[,] pixelData = new Pixel[gameData.GetLength(0),gameData.GetLength(1)];

           for(int y = 0;y < gameData.GetLength(1); y++)
           {
				for (int x = 0; x < gameData.GetLength(0); x++)
                {
                    pixelData[x, y] = new Pixel(x,y);
                    if (gameData[x, y]) {  pixelData[x, y].SetColor(250,250,250); }
                    else { pixelData[x, y].SetColor(0, 0, 0); }

                }


		   }

            return pixelData;
        }

		public static RGB GetCellColor(Pixel[,] displayData, int x, int y)
		{
			return displayData[x, y].Color;
		}
	}
   
    //public class DisplayClass
    //{
    //    private int displyX;
    //    private int displayY;  
    //    public Pixel[,] pixelData;

    //    public DisplayClass()
    //    {
    //        displyX = 60;
    //        displayY = 60;

    //        pixelData = new Pixel[displyX,displayY];
    //        InitialiseDisplay();
    //    }
       

    //    void InitialiseDisplay()
    //    {

    //        for ( int y = 0; y < displayY; y++ )
    //        { 
    //          for( int x = 0; x < displyX; x++ )
    //            {
    //                pixelData[x, y] = new Pixel(x,y);

    //                //Randomize Color 
    //                Random ran = new Random();
    //                pixelData[x, y].Color.Red = ran.Next(0, 250);
				//	pixelData[x, y].Color.Blue = ran.Next(0, 250);
				//	pixelData[x, y].Color.Green = ran.Next(0, 250);


				//}
    //        }

    //    }


    //    public RGB GetCellColor(int x, int y)
    //    {
    //        return pixelData[x, y].Color;
    //    }

    //}

    public struct Pixel
    {

        public int X;
        public int Y;

        public RGB Color;

        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
            Color = new RGB();
        }

        public void SetColor(int red ,int green,int blue)
        { 
            Color.Red = red;
            Color.Green = green;
            Color.Blue = blue;
        }

    }
  
    public struct RGB
    {
        public int Red {  get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public RGB()
        { 
            Red = 0;
            Green = 0;
            Blue = 0;
        }
        public RGB(int red, int green, int blue)
        {

            Red = CheckedInputIsInRange(red);
            Green = CheckedInputIsInRange(green);
            Blue = CheckedInputIsInRange(blue);

        }
        private int CheckedInputIsInRange(int input)
        {
            if (input < 0) { return 0; }
            else if (input > 255) { return 255; }
            else { return input; }
        }


    }
   
    
}