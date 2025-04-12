using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Worker.Domain.Interfaces.Services.Chrome;
using Worker.Service.Pages;

namespace Worker.Service.Services.Jobs;
public class Navigator : INavigator
{
    private ILogger<Navigator> _logger { get; init; }
    private IConfiguration _configuration { get; init; }

    public Navigator(ILogger<Navigator> logger,
                    IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
}