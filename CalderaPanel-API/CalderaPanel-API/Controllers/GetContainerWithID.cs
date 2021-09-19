using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Linq;

namespace CalderaPanel_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GetContainerWithIDController : ControllerBase
	{
		[HttpGet]
		public async Task<IEnumerable<ContainerListResponse>> GetAsync(string id)
		{
			DockerClient client = new DockerClientConfiguration(
			new Uri("npipe://./pipe/docker_engine"))
			 .CreateClient();

			IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(new ContainersListParameters());
			return containers.Where(container => container.ID == id);
		}
	}
}
