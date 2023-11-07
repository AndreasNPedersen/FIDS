using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "MSTestv2")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("Fly")]
[assembly: PexInstrumentAssembly("Microsoft.Extensions.Hosting.Abstractions")]
[assembly: PexInstrumentAssembly("Swashbuckle.AspNetCore.Swagger")]
[assembly: PexInstrumentAssembly("Microsoft.AspNetCore.Http.Abstractions")]
[assembly: PexInstrumentAssembly("Swashbuckle.AspNetCore.SwaggerGen")]
[assembly: PexInstrumentAssembly("System.ComponentModel")]
[assembly: PexInstrumentAssembly("Microsoft.AspNetCore.Hosting.Abstractions")]
[assembly: PexInstrumentAssembly("Swashbuckle.AspNetCore.SwaggerUI")]
[assembly: PexInstrumentAssembly("System.Linq.Queryable")]
[assembly: PexInstrumentAssembly("Microsoft.EntityFrameworkCore.SqlServer")]
[assembly: PexInstrumentAssembly("Newtonsoft.Json")]
[assembly: PexInstrumentAssembly("Microsoft.AspNetCore.Authorization.Policy")]
[assembly: PexInstrumentAssembly("Microsoft.AspNetCore.Routing")]
[assembly: PexInstrumentAssembly("System.Linq")]
[assembly: PexInstrumentAssembly("Microsoft.AspNetCore.Mvc.ApiExplorer")]
[assembly: PexInstrumentAssembly("Microsoft.Extensions.DependencyInjection.Abstractions")]
[assembly: PexInstrumentAssembly("Microsoft.EntityFrameworkCore")]
[assembly: PexInstrumentAssembly("Microsoft.AspNetCore.Cors")]
[assembly: PexInstrumentAssembly("Microsoft.AspNetCore.Mvc.Core")]
[assembly: PexInstrumentAssembly("Microsoft.AspNetCore")]
[assembly: PexInstrumentAssembly("Serilog")]
[assembly: PexInstrumentAssembly("System.Collections")]
[assembly: PexInstrumentAssembly("System.Linq.Expressions")]
[assembly: PexInstrumentAssembly("Serilog.Sinks.Grafana.Loki")]
[assembly: PexInstrumentAssembly("Microsoft.AspNetCore.Mvc")]
[assembly: PexInstrumentAssembly("Microsoft.Extensions.Logging.Abstractions")]
[assembly: PexInstrumentAssembly("System.ComponentModel.Annotations")]
[assembly: PexInstrumentAssembly("Microsoft.EntityFrameworkCore.Relational")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.Extensions.Hosting.Abstractions")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Swashbuckle.AspNetCore.Swagger")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.AspNetCore.Http.Abstractions")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Swashbuckle.AspNetCore.SwaggerGen")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.ComponentModel")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.AspNetCore.Hosting.Abstractions")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Swashbuckle.AspNetCore.SwaggerUI")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Linq.Queryable")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.EntityFrameworkCore.SqlServer")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Newtonsoft.Json")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.AspNetCore.Authorization.Policy")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.AspNetCore.Routing")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Linq")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.AspNetCore.Mvc.ApiExplorer")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.Extensions.DependencyInjection.Abstractions")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.EntityFrameworkCore")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.AspNetCore.Cors")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.AspNetCore.Mvc.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.AspNetCore")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Serilog")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Collections")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Linq.Expressions")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Serilog.Sinks.Grafana.Loki")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.AspNetCore.Mvc")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.Extensions.Logging.Abstractions")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.ComponentModel.Annotations")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.EntityFrameworkCore.Relational")]

