from confluent_kafka import Producer

from consumer_to_processor import consume_messages
from processor import process_row
from producer_to_processor import produce_message

import os


def run_pipeline():

    KAFKA_BROKER = os.getenv("KAFKA_BROKER", "localhost:9092")

    producer_config = {
        "bootstrap.servers": KAFKA_BROKER
    }

    producer = Producer(producer_config)

    try:
        for row in consume_messages("raw_data"):

            df = process_row(row)

            produce_message(
                producer,
                "processed_data",
                df
            )

    finally:
        producer.flush()


if __name__ == "__main__":
    run_pipeline()