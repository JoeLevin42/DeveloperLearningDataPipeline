from confluent_kafka import Producer

from consumer_to_processor import consume_messages
from processor import process_row
from producer_to_processor import produce_message


def run_pipeline():

    producer = Producer({
        "bootstrap.servers": "localhost:9092"
    })

    for row in consume_messages("raw-data"):

        df = process_row(row)

        produce_message(
            producer,
            "processed-data",
            df
        )


if __name__ == "__main__":
    run_pipeline()