from lxml import html

# Your HTML content
with open("41-80.html", 'r') as f:
    html_content = f.read()

# Parse the HTML
tree = html.fromstring(html_content)

# Finding all 'li' items in the 'ul'
list_items = tree.xpath('//ul[@class="list-group"]/li')

for li in list_items:
    # Extracting the question for each 'li'
    question = li.xpath('.//div[@class="term-question ql-editor"]//p/text()')
    cleaned_question = ' '.join(question).replace('\n', '').strip().replace("\"", "'").replace("\n", "")
    if cleaned_question == "" or cleaned_question == " ":
        continue
    #print("Question:", cleaned_question)
    print("{ \"QuestionType\": \"MultipleChoice\",")
    print("\"Question\": \""+cleaned_question+"\",", sep="")
    print("\"Answers\": \n[\"", sep="", end="")

    # Iterating through each answer within the same 'li'
    answers = li.xpath('.//div[@class="term-answer"]/ul/li')
    answer_index = 0
    correct_answer_indexes = []
    answer_list = []
    for answer in answers:
        answer_text = answer.xpath('.//div[@class="ql-editor"]//p/text()')
        cleaned_answer = ' '.join(answer_text).replace('\n', '').strip().replace("\"", "'").replace("\n", "")
        answer_list.append(cleaned_answer)

        # Checking for correct answer
        correct = answer.xpath('.//i[contains(@class, "fa-check")]')
        if correct:
            correct_answer_indexes.append(answer_index)
        is_correct = "Correct" if correct else "Incorrect"
        #print("Answer:", cleaned_answer, is_correct, sep=" ")
        answer_index += 1
        
    
    print('\",\"'.join(answer_list))
    print("\"],\n\"CorrectAnswerIndexes\": \n[")
    print(','.join(str(x) for x in correct_answer_indexes))
    print("],\n\"Explanation\":\"\"},")