from confluent_kafka import Consumer
import json


def consume_messages(topic):
    consumer = Consumer({
        "bootstrap.servers": "localhost:9092",
        "group.id": "raw-data-consumer",
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