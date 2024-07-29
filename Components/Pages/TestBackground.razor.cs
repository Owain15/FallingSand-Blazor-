using FallingSand.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;

namespace FallingSand.Components.Pages
{

	public partial class TestBackground
	{

		int GridSizeX = 60;
		int GridSizeY = 60;

		System.Timers.Timer timer;
		int RefreshRate = 500;

		int[,] backgroundData;
		Pixel[,] testDisplayData;

		Random random = new Random();



		protected override void OnInitialized()
		{

			backgroundData = new int[GridSizeX, GridSizeY];

			testDisplayData = new Pixel[GridSizeX, GridSizeY];

			InitialiseBackgroundData();

			InitialiseDisplay();

			StartTimmer();

			BuildDisplay();

		}


		void StartTimmer()
		{

			timer = new System.Timers.Timer(RefreshRate);
			timer.Elapsed += HandelTimeElapsedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;

		}


		private void HandelTimeElapsedEvent(Object state, System.Timers.ElapsedEventArgs e)
		{
			// check if Render, Then Process Game Data Runs Faster.

			Render();

			UpdateBackgroundData();

			BuildDisplay();

			// Render();

		}


		void InitialiseDisplay()
		{

			for (int y = 0; y < GridSizeY; y++)
			{
				for (int x = 0; x < GridSizeX; x++)
				{
					testDisplayData[x, y] = new Pixel(x, y);

					//Randomize Color 
					Random ran = new Random();
					testDisplayData[x, y].Color.Red = ran.Next(0, 250);
					testDisplayData[x, y].Color.Blue = ran.Next(0, 250);
					testDisplayData[x, y].Color.Green = ran.Next(0, 250);

				}

			}

		}

		void InitialiseBackgroundData()
		{
			int lineOffset = 0;
			int cellData = 0;

			for(int y = 0;y < GridSizeY;y++)
			{
				cellData = lineOffset; 

				for(int x = 0;x < GridSizeX;x++)
				{
					backgroundData[x, y] = cellData;
					cellData++;
					if(cellData > 4) { cellData = 0; }

				}

				lineOffset++;
				if(lineOffset > 4) { lineOffset = 0; }

			}
		}

		void UpdateBackgroundData()
		{
			for (int y = 0; y < backgroundData.GetLength(1); y++)
			{
				for (int x = 0; x < backgroundData.GetLength(0); x++)
				{
					backgroundData[x, y]++;
					if (backgroundData[x, y] > 4) { backgroundData[x, y] = 0; }
				}
			}
			
		}

		void BuildDisplay() { testDisplayData = Display.GetPixelDataFromIntArray(backgroundData); }

		void Render() { InvokeAsync(StateHasChanged); }	

	}

}