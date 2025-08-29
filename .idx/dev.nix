# see: https://developers.google.com/idx/guides/customize-idx-env
{ pkgs, ... }: {
  # Which nixpkgs channel to use.
  channel = "unstable";
  # Use https://search.nixos.org/packages to find packages
  packages = [
    pkgs.dotnet-sdk_9
    pkgs.dotnet-ef
    pkgs.sudo
    pkgs.icu
    pkgs.openssl
    pkgs.docker
    pkgs.docker-compose
  ];
  # Sets environment variables in the workspace
  env = { };
  services.docker.enable = true;
  idx = {
    # Search for the extensions you want on https://open-vsx.org/ and use "publisher.id"
    extensions = [
      "muhammad-sammy.csharp"
      "jsw.csharpextensions"
      "ms-dotnettools.vscode-dotnet-runtime"
      "DotJoshJohnson.xml"
      "eamodio.gitlens"
      "ms-azuretools.vscode-containers"
      "ms-azuretools.vscode-docker"
      "patcx.vscode-nuget-gallery"
      "PKief.material-icon-theme"
      "redhat.vscode-yaml"
      "techer.open-in-browser"
      "zxh404.vscode-proto3"
    ];
  };
}
