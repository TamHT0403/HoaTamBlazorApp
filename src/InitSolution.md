dotnet new sln -n HoaTamApp

### Shared Kernel
dotnet new classlib -o src/SharedKernel/HTA.Shared/Shared.Domain
dotnet new classlib -o src/SharedKernel/HTA.Shared/Shared.Application
dotnet new classlib -o src/SharedKernel/HTA.Shared/Shared.Infrastructure

dotnet sln HoaTamApp.sln add src/SharedKernel/HTA.Shared/Shared.Domain/Shared.Domain.csproj
dotnet sln HoaTamApp.sln add src/SharedKernel/HTA.Shared/Shared.Application/Shared.Application.csproj
dotnet sln HoaTamApp.sln add src/SharedKernel/HTA.Shared/Shared.Infrastructure/Shared.Infrastructure.csproj

dotnet add src/SharedKernel/HTA.Shared/Shared.Application/Shared.Application.csproj reference src/SharedKernel/HTA.Shared/Shared.Domain/Shared.Domain.csproj
dotnet add src/SharedKernel/HTA.Shared/Shared.Infrastructure/Shared.Infrastructure.csproj reference src/SharedKernel/HTA.Shared/Shared.Domain/Shared.Domain.csproj

### WebBlazor UI
dotnet new classlib -o src/Web/HTA.UI/UI.Domain
dotnet new classlib -o src/Web/HTA.UI/UI.Application
dotnet new blazorwasm -o src/Web/HTA.UI/UI.Blazorwasm

dotnet sln HoaTamApp.sln add src/Web/HTA.UI/UI.Domain/UI.Domain.csproj
dotnet sln HoaTamApp.sln add src/Web/HTA.UI/UI.Blazorwasm/UI.Blazorwasm.csproj
dotnet sln HoaTamApp.sln add src/Web/HTA.UI/UI.Application/UI.Application.csproj

dotnet add src/Web/HTA.UI/UI.Application/UI.Application.csproj reference src/Web/HTA.UI/UI.Domain/UI.Domain.csproj 
dotnet add src/Web/HTA.UI/UI.Blazorwasm/UI.Blazorwasm.csproj reference src/Web/HTA.UI/UI.Application/UI.Application.csproj

### Account service
dotnet new webapi -o src/Microservices/Account/Account.API
dotnet new classlib -o src/Microservices/Account/Account.Domain
dotnet new classlib -o src/Microservices/Account/Account.Application
dotnet new classlib -o src/Microservices/Account/Account.Infrastructure

dotnet sln HoaTamApp.sln add src/Microservices/Account/Account.API/Account.API.csproj
dotnet sln HoaTamApp.sln add src/Microservices/Account/Account.Domain/Account.Domain.csproj
dotnet sln HoaTamApp.sln add src/Microservices/Account/Account.Application/Account.Application.csproj
dotnet sln HoaTamApp.sln add src/Microservices/Account/Account.Infrastructure/Account.Infrastructure.csproj

dotnet add src/Microservices/Account/Account.Application/Account.Application.csproj reference src/Microservices/Account/Account.Domain/Account.Domain.csproj
dotnet add src/Microservices/Account/Account.Infrastructure/Account.Infrastructure.csproj reference src/Microservices/Account/Account.Domain/Account.Domain.csproj
dotnet add src/Microservices/Account/Account.API/Account.API.csproj reference src/Microservices/Account/Account.Application/Account.Application.csproj
dotnet add src/Microservices/Account/Account.API/Account.API.csproj reference src/Microservices/Account/Account.Infrastructure/Account.Infrastructure.csproj
dotnet add src/Microservices/Account/Account.Domain/Account.Domain.csproj reference src/SharedKernel/HTA.Shared/Shared.Domain/Shared.Domain.csproj
dotnet add src/Microservices/Account/Account.Application/Account.Application.csproj reference src/SharedKernel/HTA.Shared/Shared.Application/Shared.Application.csproj
dotnet add src/Microservices/Account/Account.Infrastructure/Account.Infrastructure.csproj reference src/SharedKernel/HTA.Shared/Shared.Infrastructure/Shared.Infrastructure.csproj

### Product service
dotnet new webapi -o src/Microservices/Product/Product.API
dotnet new classlib -o src/Microservices/Product/Product.Domain
dotnet new classlib -o src/Microservices/Product/Product.Application
dotnet new classlib -o src/Microservices/Product/Product.Infrastructure

dotnet sln HoaTamApp.sln add src/Microservices/Product/Product.API/Product.API.csproj
dotnet sln HoaTamApp.sln add src/Microservices/Product/Product.Domain/Product.Domain.csproj
dotnet sln HoaTamApp.sln add src/Microservices/Product/Product.Application/Product.Application.csproj
dotnet sln HoaTamApp.sln add src/Microservices/Product/Product.Infrastructure/Product.Infrastructure.csproj

dotnet add src/Microservices/Product/Product.Application/Product.Application.csproj reference src/Microservices/Product/Product.Domain/Product.Domain.csproj
dotnet add src/Microservices/Product/Product.Infrastructure/Product.Infrastructure.csproj reference src/Microservices/Product/Product.Domain/Product.Domain.csproj
dotnet add src/Microservices/Product/Product.API/Product.API.csproj reference src/Microservices/Product/Product.Application/Product.Application.csproj
dotnet add src/Microservices/Product/Product.API/Product.API.csproj reference src/Microservices/Product/Product.Infrastructure/Product.Infrastructure.csproj
dotnet add src/Microservices/Product/Product.Domain/Product.Domain.csproj reference src/SharedKernel/HTA.Shared/Shared.Domain/Shared.Domain.csproj
dotnet add src/Microservices/Product/Product.Application/Product.Application.csproj reference src/SharedKernel/HTA.Shared/Shared.Application/Shared.Application.csproj
dotnet add src/Microservices/Product/Product.Infrastructure/Product.Infrastructure.csproj reference src/SharedKernel/HTA.Shared/Shared.Infrastructure/Shared.Infrastructure.csproj


### Gateway
dotnet new webapi   -o src/APIGateway/HTA.Gateway/Gateway.API
dotnet new classlib -o src/APIGateway/HTA.Gateway/Gateway.Domain
dotnet new classlib -o src/APIGateway/HTA.Gateway/Gateway.Application
dotnet new classlib -o src/APIGateway/HTA.Gateway/Gateway.Infrastructure

dotnet sln HoaTamApp.sln add src/APIGateway/HTA.Gateway/Gateway.API/Gateway.API.csproj
dotnet sln HoaTamApp.sln add src/APIGateway/HTA.Gateway/Gateway.Domain/Gateway.Domain.csproj
dotnet sln HoaTamApp.sln add src/APIGateway/HTA.Gateway/Gateway.Application/Gateway.Application.csproj
dotnet sln HoaTamApp.sln add src/APIGateway/HTA.Gateway/Gateway.Infrastructure/Gateway.Infrastructure.csproj

dotnet add src/APIGateway/HTA.Gateway/Gateway.Domain/Gateway.Domain.csproj reference src/SharedKernel/HTA.Shared/Shared.Domain/Shared.Domain.csproj
dotnet add src/APIGateway/HTA.Gateway/Gateway.Application/Gateway.Application.csproj reference src/APIGateway/HTA.Gateway/Gateway.Domain/Gateway.Domain.csproj
dotnet add src/APIGateway/HTA.Gateway/Gateway.Application/Gateway.Application.csproj reference src/SharedKernel/HTA.Shared/Shared.Application/Shared.Application.csproj
dotnet add src/APIGateway/HTA.Gateway/Gateway.Infrastructure/Gateway.Infrastructure.csproj reference src/APIGateway/HTA.Gateway/Gateway.Domain/Gateway.Domain.csproj
dotnet add src/APIGateway/HTA.Gateway/Gateway.Infrastructure/Gateway.Infrastructure.csproj reference src/SharedKernel/HTA.Shared/Shared.Infrastructure/Shared.Infrastructure.csproj
dotnet add src/APIGateway/HTA.Gateway/Gateway.API/Gateway.API.csproj reference src/APIGateway/HTA.Gateway/Gateway.Application/Gateway.Application.csproj
dotnet add src/APIGateway/HTA.Gateway/Gateway.API/Gateway.API.csproj reference src/APIGateway/HTA.Gateway/Gateway.Infrastructure/Gateway.Infrastructure.csproj