EXTERNAL SetNextStep(nNextStep, strQuestName)
EXTERNAL DoDialogueRoll(nStatRequired, strStatType)
EXTERNAL AddMorality(nChange)

CONST _questName = "Ore Mining"
VAR _questStep = 0
VAR _rollSuccess = false

{_questStep :
- 0: -> Step0
- 1: -> Step1
- 2: -> Step2
- 3: -> Step3
}


===Step0===
Hello, dear. #SPEAKER:Grandma
I heard iron is good for the body.
Can you get your old granny some iron?

+ [YES]
~ SetNextStep(1, _questName) 
Thank you dear.  
-> END

+ [NO]
~ SetNextStep(-1, _questName) //quest was never started
Oh, how heartless.
-> END


===Step1===
Wait, did granny want iron or gold? #SPEAKER:Narrator
*[Iron (5 INT)]
~ DoDialogueRoll(5, "INT")
You think you recall it being iron. //Pass the result in
{ _rollSuccess :
    [{_rollSuccess}] Your memory is sharp as ever. 
    ~ SetNextStep(3, _questName)
    We can go back now.
    ->END
    -else:
    [{_rollSuccess}] No. You might be misremembering.
    ->Step1
}
+[Gold]
- Hmm...
Better safe than sorry I guess.
But mining is hard.
Why don't we try a little alchemy to make some gold?
+[Give it a try (10 WIS)]
~ DoDialogueRoll(10, "WIS")
You bring out a bottle of mountain dew and begin chanting.
{ _rollSuccess :
    [{_rollSuccess}] Huzzah!
    Alchemy has provided you plenty of gold.
    ~ SetNextStep(3, _questName)
    Let's head back to granny now.
    ->END
    -else:
    [{_rollSuccess}] Boom!
    Perhaps, alchemy requires an equivalent exchange.
    Save it as a disccusion for another day.
}
+[NO. Magic is sacred]
- Anyhows... Let's get digging. 
~ SetNextStep(2,  _questName)
We yearn for the mines.
->END


===Step2===
That's enough, gold. #SPEAKER:Narrator
~ SetNextStep(3,  _questName)
~ AddMorality(2)
Back to granny.
->END

===Step3===
Thank you very much, dear. #SPEAKER:Grandma
With this granny can last for a year.
~ SetNextStep(4,  _questName)
~ AddMorality(1)
This old lady won't be another victim to pharmaceutical greed.
->END