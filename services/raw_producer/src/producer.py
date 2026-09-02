from pathlib import Path
import csv
import json
from confluent_kafka import Producer


def produce_file(file_path, topic):
    try:
        producer = Producer({
            "bootstrap.servers": "localhost:9092"
        })

        with open(file_path, "r", newline="", encoding="utf-8") as file:
            reader = csv.DictReader(file)

            for row in reader:
                message = json.dumps(row)

                producer.produce(
                    topic=topic,
                    value=message.encode("utf-8")
                )

        producer.flush()

    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    csv_path = (
        Path(__file__).resolve().parents[2]
        / "data"
        / "developer_ai_learning_raw.csv"
    )

    produce_file(csv_path, "raw_data")