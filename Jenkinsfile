Jenkinsfile {
    pipeline {
        agent any
        triggers {
            pollSCM("* * * * *") //when it should pull from version control. the stars is at any time .
        }
        environment {
            DEPLOY_NUMBER = "${BUILD_NUMBER}"
        }
        stages {
            stage("Continous integration") {
                steps {
                    sh "dotnet build" //to run a comand line task. jenkin looks at exit code to determine if it fails or not. exit code 0 is fine
                    sh "docker compose build"
                }
            }
            stage("Continous delievery") {
                steps {
                    withCredentials([usernamePassword(credentialsId: "DockerHub", passwordVariable: "DH_PASSWORD", usernameVariable: "DH_USERNAME")]){
                        sh 'docker login -u $DH_USERNAME -p $DH_PASSWORD' //single quote makes it impossible to
                        sh "docker compose push"
                    }
                }
            }
            stage("Continous deploy deploy") {
                steps {
                    build job: "RockPaperScissor-Rollback", parameters: [[$class: "StringParameterValue", name: "DEPLOY_NUMBER", value: "${BUILD_NUMBER}"]]
                    //sh "docker compose rm" //to remove the old one
                    //sh "docker compose up -d" //deatch mode , the process will end and everything is fine
                }
            }
        }
    }
}