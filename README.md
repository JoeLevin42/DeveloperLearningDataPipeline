# Developer Learning Data Pipeline

## Run the project

Clone the repository:

```bash
git clone <repository-url>
cd DeveloperLearningDataPipeline
```

Start all services with Docker:

```bash
docker compose up --build -d
```

Check that all containers are running:

```bash
docker compose ps
```

## API

The API is available at:

```text
http://localhost:8080
```

Swagger:

```text
http://localhost:8080/swagger
```

To stop the project:

```bash
docker compose down
```
