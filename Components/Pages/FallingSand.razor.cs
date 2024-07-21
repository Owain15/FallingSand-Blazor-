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
        int RefreshRate = 100;

        //List<(int,int)> SandList = new List<(int,int)>();
		bool[,] gameData;
        Pixel[,] displayData;

        Random random = new Random();
   
       

        protected override void OnInitialized()
        {

            //SandList.Clear();

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

      
        public void HandelTimeElapsedEvent(Object state , System.Timers.ElapsedEventArgs e)
        {

            gameData = UpdateGameData();

            BuildDisplay();

            Render();
          
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

        bool[,] UpdateGameData()
        {
            bool[,] updatedGameData = new bool[GridSizeX, GridSizeY];

            for(int y = 0; y < gameData.GetLength(1); y++)
            {
                for(int x = 0; x < gameData.GetLength(0); x++)
                {
                    if (gameData[x, y])
                    {
                        if( y + 1 > gameData.GetLength(1)-1) { updatedGameData[x, y] = true; }
                        else if (!gameData[x, y + 1]) { updatedGameData[x, y + 1] = true;}
                        else if (gameData[x, y + 1]) {updatedGameData = PileSand(x,y, gameData,updatedGameData); }
                    }
                }

            }
            return updatedGameData;

        }

        bool[,] PileSand(int x, int y, bool[,] gameData, bool[,] updatedGameData)
        {

            var tryLeftFirst = true;
            if (random.Next() > 0.5) { tryLeftFirst = false; }

            if (x - 1 >= 0 && tryLeftFirst)
            {

                if (!gameData[x - 1, y + 1]) { updatedGameData[x - 1, y + 1] = true; }
                else if(x + 1 <= gameData.GetLength(0) - 1 && !gameData[x + 1, y + 1])
                { updatedGameData[x + 1, y + 1] = true; }
                else{ updatedGameData[x, y] = true; }
            }
            else if (x + 1 <= gameData.GetLength(0)-1 && !tryLeftFirst)
            {
				if (x + 1 <= gameData.GetLength(0) - 1 && !gameData[x + 1, y + 1])
				{ updatedGameData[x + 1, y + 1] = true; }
                else if (x -1 >=0 &&!gameData[x - 1, y + 1]) { updatedGameData[x - 1, y + 1] = true; }
				else { updatedGameData[x, y] = true; }

			}
            else { Console.WriteLine("ERORR.NextSandLocationNotFound!"); }
              
          

            return updatedGameData;
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