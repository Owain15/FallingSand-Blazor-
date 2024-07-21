using Microsoft.AspNetCore.Components.Web;

namespace FallingSand.Components.Pages
{
	public partial class TestTimer
	{
		// Properties
	
		private TimeSpan timeSpan;
		private DateTime startTime;
		private Timer timer;

		private bool isRunning = false;


		// Methods
        
		private void StopwatchStart()
		{
			if (!isRunning)
			{
				isRunning = true;
			    
				startTime = DateTime.Now-timeSpan;
				timer = new Timer(UpdateTime,null,0,1000);
			}
		}

		private void StopwatchStop()
		{
			if (isRunning)
			{
				isRunning = false;

				timer?.Dispose();
			}
		}

		private void StopwatchReset()
		{
			StopwatchStop();

			timeSpan = TimeSpan.Zero;
		}
		private void UpdateTime(object state)
		{
			timeSpan = DateTime.Now - startTime;
			InvokeAsync(StateHasChanged);
		}

	}
}