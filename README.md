# OpenReportHtml
The API to render HTML using razor. You can get personal HTML page and Model.

The project is in net5.0.

## Building the project

Clone the project ```git clone https://github.com/willimar/OpenReportHtml.git```

Open the solution ```OpenReportHtml.sln``` and press Ctrl+Shift+B. The project will restore all dependences and build the application. To debug application execute F5 and wait the brownser open. After open the swagger ui will show the controller options.

![](images/swagger-uipng.png)

## Compiling new docker image

The repository is automatic, when made new push code to main the Docker Hub was compile new image, but to manual compile image use Dockerfile.
Command to build image: "```docker build -t open-report-html .```"

To execute the image use the command ```docker run -p 8003:80 open-report-html```.

To see the resulted open url ```http://localhost:8003``` in your webbrownser.
