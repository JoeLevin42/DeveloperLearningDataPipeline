FROM python:3.13.13-slim

WORKDIR /producerApp

COPY services/raw_producer/requirements.txt ./

RUN pip install --no-cache-dir -r requirements.txt

COPY services/raw_producer/src ./src

COPY data ./data

CMD ["python", "src/producer.py"]