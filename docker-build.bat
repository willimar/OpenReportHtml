docker build -t open-report-html .

heroku login
heroku container:login

docker tag open-report-html registry.heroku.com/open-report-html/web
docker push registry.heroku.com/open-report-html/web

heroku container:release web -a open-report-html