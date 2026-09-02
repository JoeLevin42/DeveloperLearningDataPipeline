# processor.py

import json
from consumer_to_processor import get_message


def process_message(msg):
    # Convert Kafka message to Python data
    data = json.loads(msg.value().decode("utf-8"))

    # Business logic
    if not validate_data(data):
        return "invalid", data

    processed_data = apply_business_logic(data)

    return "valid", processed_data


def validate_data(data):
    # Your validation rules
    return True


def apply_business_logic(data):
    # Your business logic
    return data


def run():
    while True:

        # Get ONE message from the consumer
        msg = get_message()

        if msg is None:
            continue

        # Process that ONE message
        topic, data = process_message(msg)

        # Now you can send it to the appropriate Kafka topic
        if topic == "valid":
            print("Send to valid:", data)

        else:
            print("Send to invalid:", data)


if __name__ == "__main__":
    run()