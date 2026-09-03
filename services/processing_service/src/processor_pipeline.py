from confluent_kafka import Producer

from consumer_to_processor import consume_messages
from processor import process_row
from producer_to_processor import produce_message


def run_pipeline():
    try:
        producer = Producer({
            "bootstrap.servers": "localhost:9092"
        })

        for row in consume_messages("raw_data"):

            df = process_row(row)
            
            produce_message(
                producer,
                "processed-data",
                df
            )
    finally:
        producer.flush()


if __name__ == "__main__":
    run_pipeline()
    