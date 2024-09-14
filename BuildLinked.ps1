# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/sonicheroes.amyespio.abilities/*" -Force -Recurse
dotnet publish "./sonicheroes.amyespio.abilities.csproj" -c Release -o "$env:RELOADEDIIMODS/sonicheroes.amyespio.abilities" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location