pipeline {
    agent any
    triggers {
        pollSCM("* * * * *")
    }
    environment {
        DEPLOY_NUMBER = "${BUILD_NUMBER}"
    }
    stages {
        stage("C. Int") {
            steps {
                sh "dotnet build"
                sh "docker compose build"
            }
        }
        stage("C. Del") {
            steps {
                withCredentials([usernamePassword(credentialsId: "Hub", passwordVariable: "DH_PASSWORD", usernameVariable: "DH_USERNAME")]) {
                    sh 'docker login -u $DH_USERNAME -p $DH_PASSWORD'
                    sh "docker compose push"
                }
            }
        }
        stage("Deploy") {
            steps {
                sh "docker compose rm"
                sh "docker compose up -d"
            }
        }
    }
}