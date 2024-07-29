using FallingSand.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        int RefreshRate = 15;

		bool[,] gameData;
        Pixel[,] displayData;

        Random random = new Random();
   
       

        protected override void OnInitialized()
        {

            gameData = new bool[GridSizeX,GridSizeY];
          
            displayData = new Pixel[GridSizeX,GridSizeY];

		    InitialiseDisplay();

            StartTimmer();

            //test sand. Remove.
            gameData[10, 10] = true;
			gameData[10, 11] = true;
			gameData[20, 10] = true;
			gameData[20, 11] = true;

		    BuildDisplay();
			
		}


		void StartTimmer()
        {

            timer = new System.Timers.Timer(RefreshRate);
			timer.Elapsed += HandelTimeElapsedEvent;
			timer.AutoReset = true;
            timer.Enabled = true;
            
        }

      
        private void HandelTimeElapsedEvent(Object state , System.Timers.ElapsedEventArgs e)
        {
            // check if Render, Then Process Game Data Runs Faster.

            Render();

            UpdateGameData();

            BuildDisplay();

           // Render();
          
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

        void BuildDisplay()    { displayData = Display.GetPixelDataFromGameState(gameData); }

        void Render()   { InvokeAsync(StateHasChanged); }

        void UpdateGameData()
        { 

            for(int y = gameData.GetLength(1)-1; y > -1 ; y--)
            {
                for(int x = gameData.GetLength(0)-1; x > -1 ; x--)
                {
                    if (gameData[x, y])
                    {
                        if( y + 1 > gameData.GetLength(1)-1) { break; }
                        else if (!gameData[x, y + 1]) { gameData[x, y + 1] = true; gameData[x, y] = false; }
                        else if (gameData[x, y + 1]) {PileSand(x,y); }
                    }
                }

            }
        

        }

        void PileSand(int x, int y)
        {

            var tryLeftFirst = true;
            if (random.NextDouble() > 0.5) { tryLeftFirst = false; }

            if (x - 1 >= 0 && tryLeftFirst)
            {

                if (!gameData[x - 1, y + 1]) { gameData[x - 1, y + 1] = true; gameData[x, y] = false; }
                else if(x + 1 <= gameData.GetLength(0) - 1 && !gameData[x + 1, y + 1])
                { gameData[x + 1, y + 1] = true; gameData[x, y] = false; }
             
            }
            else if (x + 1 <= gameData.GetLength(0)-1 && !tryLeftFirst)
            {
				if (x + 1 < gameData.GetLength(0) && !gameData[x + 1, y + 1])
				{ gameData[x + 1, y + 1] = true; gameData[x, y] = false; }
                else if (x -1 >=0 && !gameData[x - 1, y + 1]) 
                { gameData[x - 1, y + 1] = true; gameData[x, y] = false; }

			}

        }

        void AddSand(int x, int y){ if (!gameData[x, y]) { gameData[x, y] = true; } }

		
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

        public static Pixel[,] GetPixelDataFromIntArray(int[,] backgroundData)
		{
			Pixel[,] pixelData = new Pixel[backgroundData.GetLength(0), backgroundData.GetLength(1)];

			for (int y = 0; y < backgroundData.GetLength(1); y++)
			{
				for (int x = 0; x < backgroundData.GetLength(0); x++)
				{
					pixelData[x, y] = new Pixel(x, y);
                    switch(backgroundData[x,y])
                    {
                        case 0: { pixelData[x, y].SetColor(50, 0, 50); } break;
						case 1: { pixelData[x, y].SetColor(100, 0, 100); } break;
						case 2: { pixelData[x, y].SetColor(150, 0, 150); } break;
						case 3: { pixelData[x, y].SetColor(200, 0, 200); } break;
						case 4: { pixelData[x, y].SetColor(250, 0, 250); } break;
						default: { Console.WriteLine($"Pixel Reference Out Of Range. {x},{y}. Value{backgroundData[x,y]}."); }break;

                    }

				}


			}

			return pixelData;
		}

	}
   
   

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