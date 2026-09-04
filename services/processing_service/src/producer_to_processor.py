from confluent_kafka import Producer
import json
import os

def produce_message(producer, topic, df):


    KAFKA_BROKER = os.getenv("KAFKA_BROKER", "localhost:9092")
    data = df.to_dict(orient="records")[0]

    producer.produce(
        topic,
        value=json.dumps(data)
    )
    print(f"Consumed row: {json.dumps(data)}")

    