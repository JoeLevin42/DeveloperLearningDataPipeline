from confluent_kafka import Consumer
import json
import pandas as pd

from consumer_to_processor import consume_messages


def convert_to_dataframe(row):
    return pd.DataFrame([row])

def convert_column_to_numeric(df):
    df = df['YearsCode'] = df['YearsCode'].astype('Int64')
    return df

def split_multiselect(value):
    if pd.isna(value):
        return None
    return value.Split(";")

def clean_column(df):
    df['LearnCode'] = df['LearnCode'].apply(split_multiselect)
    df['AILearnHow'] = df['AILearnHow'].apply(split_multiselect)
    return df

def enrich
    



