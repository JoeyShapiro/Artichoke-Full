docker build -t artichoke .
docker run -p 6060:5000 artichoke
docker compose up --build