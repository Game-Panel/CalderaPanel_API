using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace CalderaPanel_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GetLogContainerWithIDController : ControllerBase
	{
		[HttpGet]
		public async Task<IEnumerable<char>> GetAsync(string id)
		{
			DockerClient client = new DockerClientConfiguration(
			new Uri("npipe://./pipe/docker_engine"))
			 .CreateClient();

			try
			{
				var logs = await client.Containers.GetContainerLogsAsync(id, new ContainerLogsParameters { ShowStdout = true, ShowStderr = true }, CancellationToken.None);
				using (var reader = new StreamReader(logs))
				{
					return reader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}
	}
}
