EXTERNAL SetNextStep(nNextStep)

VAR _questStep = 0

{_questStep :
- 0: -> Step0
- 1: -> Step1
- 2: -> Step2
- 3: -> Step3
}


===Step0===
Hello, dear.
I heard iron is good for the body.
Can you get your old granny some iron?

+ [YES]
~ SetNextStep(1) 
Thank you dear.  
-> END

+ [NO]
~ SetNextStep(-1) //quest was never started
Oh, how heartless.
-> END


===Step1===
Wait, did granny want iron or gold?
*[Gold]
*[Gold]
*[Gold]
- Hmm...
~ SetNextStep(2)
Better safe than sorry I guess.
->END


===Step2===
That's enough, gold.
~ SetNextStep(3)
Back to granny.
->END

===Step3===
Thank you very much, dear.
With this granny can last for a year.
~ SetNextStep(4)
This is old lady won't be another victim to pharmaceutical greed.
->END