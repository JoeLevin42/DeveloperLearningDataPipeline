import pandas as pd


TECH_DOC = "Technical documentation (is generated for/by the tool or system)"
AI_CODEGEN = "AI CodeGen tools or AI-enabled apps"
STACK_OVERFLOW = "Stack Overflow or Stack Exchange"

def convert_to_dataframe(row):
    return pd.DataFrame([row])

def convert_column_to_numeric(df):
    df['YearsCode'] = df['YearsCode'].astype('Int64')
    return df

def split_multiselect(value):
    if pd.isna(value):
        return None
    return value.split(";")

def clean_column(df):
    df['LearnCode'] = df['LearnCode'].apply(split_multiselect)
    df['AILearnHow'] = df['AILearnHow'].apply(split_multiselect)
    return df

## this is the enrichment level 

def experience_level(years_code):
    # Check for missing values (None or pd.NA)
    if pd.isna(years_code):
        return "Unknown"
    if years_code <= 2:
        return "Beginner"
    if years_code <= 5:
        return "Early Career"
    if years_code <= 10:
        return "Experienced"
    return "Highly Experienced"

def has_option(options, target):
    if options is None:
        return False
    return target in options

def enrich_experience(df):
    df['experienceLevel'] = df['YearsCode'].apply(experience_level)
    return df


def enrich_usesDocumentation(df):
    df['usesDocumentation'] = df['LearnCode'].apply(lambda x: has_option(x, TECH_DOC))
    return df

def enrich_usesAIForLearning(df):
    df['usesAIForLearning'] = df['LearnCode'].apply(lambda x: has_option(x, AI_CODEGEN))
    return df

def enrich_usesStackOverflow(df):
    df['usesStackOverflow'] = df['LearnCode'].apply(lambda x: has_option(x, STACK_OVERFLOW))
    return df

def rename_columns_names(df):
    df = df.rename(columns={
    'ResponseId': 'responseId',
    'Age': 'age',
    'YearsCode': 'yearsCode',
    'DevType': 'devType',
    'LearnCodeChoose': 'learnCodeChoose',
    'LearnCode': 'learningMethods',
    'LearnCodeAI': 'learnCodeAI',
    'AILearnHow': 'aiLearningMethods',
    'AISelect': 'aiUsage',
    'AIAcc': 'aiTrust',
    'AISent': 'aiSentiment',
})
    return df

def process_row(row):

    df = convert_to_dataframe(row)

    df = convert_column_to_numeric(df)

    df = clean_column(df)

    #Enrichment
    df = enrich_experience(df)
    df = enrich_usesDocumentation(df)
    df = enrich_usesAIForLearning(df)
    df = enrich_usesStackOverflow(df)

    # Rename columns
    df = rename_columns_names(df)

    return df



        