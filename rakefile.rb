require 'albacore'
require 'nuget_helper'

$dir = File.join(File.dirname(__FILE__),'src')
$sln = File.join($dir, "Isop.Server.sln")

desc "Install missing NuGet packages."
task :restore do
  NugetHelper.exec("restore #{$sln}")
end

desc "build"
build :build => [:restore] do |msb|
  msb.prop :configuration, :Debug
  msb.prop :platform, "Mixed Platforms"
  msb.target = :Rebuild
  msb.be_quiet
  msb.nologo
  msb.sln = $sln
end

build :build_release => [:restore] do |msb|
  msb.prop :configuration, :Release
  msb.prop :platform, "Mixed Platforms"
  msb.target = :Rebuild
  msb.be_quiet
  msb.nologo
  msb.sln = $sln
end


task :default => ['build']

desc "test using console"
test_runner :test => [:build] do |runner|
  runner.exe = NugetHelper.nunit_path
  files = Dir.glob(File.join($dir, "*Tests", "**", "bin", "Debug", "*Tests.dll"))
  runner.files = files 
end

task :client_nugetpack => [:build_release] do |nuget|
  cd File.join($dir, "Isop.Client") do
    NugetHelper::exec "pack Isop.Client.csproj"
  end
end

task :client_nugetpush do |nuget|
  cd File.join($dir, "Isop.Client") do
    latest = NugetHelper.last_version(Dir.glob("Isop.Client.*.nupkg"))
    NugetHelper::exec("push #{latest}")
  end
end