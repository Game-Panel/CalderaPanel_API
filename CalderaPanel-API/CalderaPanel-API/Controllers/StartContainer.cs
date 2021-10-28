using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Threading.Tasks;

namespace CalderaPanel_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class StartContainerController : ControllerBase
	{
		[HttpPost]
		public async Task<IEnumerable<char>> PostAsync(string id)
		{
			DockerClient client = new DockerClientConfiguration(
			new Uri("npipe://./pipe/docker_engine"))
			 .CreateClient();

			try
			{
				var start = await client.Containers.StartContainerAsync(id, new ContainerStartParameters());
				return start.ToString();
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}
	}
}
