pipeline {
  agent { label 'windows' }

  options {
    timestamps()
    disableConcurrentBuilds()
  }

  parameters {
    booleanParam(
      name: 'RUN_INTEGRATION_TESTS',
      defaultValue: false,
      description: 'Run integration tests using scripts/run-integration.ps1 (requires Docker on the agent).'
    )
  }

  environment {
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
    DOTNET_CLI_TELEMETRY_OPTOUT = '1'
    NUGET_XMLDOC_MODE = 'skip'
  }

  stages {
    stage('Checkout') {
      steps {
        checkout scm
      }
    }

    stage('Restore') {
      steps {
        powershell 'dotnet restore geoserver.net.slnx'
      }
    }

    stage('Build') {
      steps {
        powershell 'dotnet build geoserver.net.slnx -c Release --no-restore'
      }
    }

    stage('Build net48') {
      steps {
        powershell 'dotnet build src/GeoServer.Net/GeoServer.Net.csproj -c Release -f net48 --no-restore'
      }
    }

    stage('Unit Tests') {
      steps {
        powershell 'dotnet test tests/GeoServer.Net.Tests/GeoServer.Net.Tests.csproj -c Release --no-build'
      }
    }

    stage('Integration Tests') {
      when {
        expression { return params.RUN_INTEGRATION_TESTS }
      }
      steps {
        powershell '.\\scripts\\run-integration.ps1 -DetailedTestOutput'
      }
    }

    stage('Pack') {
      steps {
        powershell 'dotnet pack src/GeoServer.Net/GeoServer.Net.csproj -c Release --no-build -o .\\artifacts\\packages'
      }
    }
  }

  post {
    always {
      archiveArtifacts artifacts: 'artifacts/packages/**/*', allowEmptyArchive: true, fingerprint: true
    }
  }
}
