from confluent_kafka import Producer
import json


def produce_message(producer, topic, df):

    data = df.to_dict(orient="records")[0]

    producer.produce(
        topic,
        value=json.dumps(data)
    )

    producer.flush()