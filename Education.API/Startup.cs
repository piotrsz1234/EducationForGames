using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationLib.APIHelper;
using EducationLib.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Education.API {
	public class Startup {
		public Startup (IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices (IServiceCollection services) {
			services.AddControllers ();
			MongoClient client = new MongoClient (Configuration.GetConnectionString ("MongoDB"));
			var database = client.GetDatabase ("Education");
			services.AddSingleton (database.GetCollection<User>("Users"));
			services.AddSingleton (database.GetCollection<Question>("Questions"));
			services.AddSingleton (database.GetCollection<QuestionAnswer>("Answers"));
			services.AddSingleton (database.GetCollection<RegistrationCode>("Codes"));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment ()) {
				app.UseDeveloperExceptionPage ();
			}

			app.UseHttpsRedirection ();

			app.UseRouting ();

			app.UseAuthorization ();
			
			app.UseEndpoints (endpoints => {
				endpoints.MapControllers ();
			});
		}
	}
}
