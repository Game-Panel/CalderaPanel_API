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
	public class RemoveContainerController : ControllerBase
	{
		[HttpPost]
		public async Task<IEnumerable<char>> PostAsync(string id, ContainerRemoveParameters data)
		{
			DockerClient client = new DockerClientConfiguration(
			new Uri("npipe://./pipe/docker_engine"))
			 .CreateClient();

			try
			{
				await client.Containers.RemoveContainerAsync(id, data);
				return "Success";
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}
	}
}
