# see: https://developers.google.com/idx/guides/customize-idx-env
{ pkgs, ... }: {
  # Which nixpkgs channel to use.
  channel = "unstable";
  # Use https://search.nixos.org/packages to find packages
  packages = [ pkgs.dotnet-sdk_9  pkgs.dotnet-ef];
  # Sets environment variables in the workspace
  env = { };
  idx = {
    # Search for the extensions you want on https://open-vsx.org/ and use "publisher.id"
    extensions = [ "muhammad-sammy.csharp"  "ms-dotnettools.vscode-dotnet-runtime" "chrisatwindsurf.csharpextension" "DotJoshJohnson.xml" "eamodio.gitlens" "ms-azuretools.vscode-containers" "ms-azuretools.vscode-docker" "patcx.vscode-nuget-gallery" "PKief.material-icon-theme" "redhat.vscode-yaml" "techer.open-in-browser" "zxh404.vscode-proto3"];
    # Enable previews and customize configuration
    previews = {
      enable = true;
      previews = {
        web = {
          command = [ "dotnet" "watch" "--urls=http://localhost:$PORT" ];
          manager = "web";
        };
      };
    };
  };
}
