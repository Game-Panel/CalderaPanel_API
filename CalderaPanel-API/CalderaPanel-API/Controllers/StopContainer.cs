using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Threading;

namespace CalderaPanel_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class StopContainerController : ControllerBase
	{
		[HttpPost]
		public async Task<IEnumerable<char>> PostAsync(string id)
		{
			DockerClient client = new DockerClientConfiguration(
			new Uri("npipe://./pipe/docker_engine"))
			 .CreateClient();

			try
			{
				var stop = await client.Containers.StopContainerAsync(id, new ContainerStopParameters { WaitBeforeKillSeconds = 30 }, CancellationToken.None);
				return stop.ToString();
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}
	}
}
