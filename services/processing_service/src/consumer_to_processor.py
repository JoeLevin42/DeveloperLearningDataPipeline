from confluent_kafka import Consumer
import json
import os


def consume_messages(topic):
    KAFKA_BROKER = os.getenv("KAFKA_BROKER", "localhost:9092")

    consumer = Consumer({
        "bootstrap.servers": KAFKA_BROKER,
        "group.id": "processor",
        "auto.offset.reset": "earliest"
    })

    consumer.subscribe([topic])

    try:
        while True:
            message = consumer.poll(1.0)

            if message is None:
                continue

            if message.error():
                print(f"Error: {message.error()}")
                continue

            row = json.loads(message.value().decode("utf-8"))

            print(f"Consumed row: {row}")

            yield row

    except KeyboardInterrupt:
        pass

    finally:
        consumer.close()