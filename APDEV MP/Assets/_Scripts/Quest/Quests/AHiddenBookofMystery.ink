EXTERNAL SetNextStep(nNextStep, strQuestName)
EXTERNAL DoDialogueRoll(nStatRequired, strStatType)
EXTERNAL RewardItem(strItemName)

CONST _questName = "A Hidden Book of Mystery"
VAR _questStep = 0
VAR _rollSuccess = false

{_questStep :
- 0: -> Step0
- 1: -> Step1
- 2: -> Step2
- 3: -> Step3
}


===Step0===
As you inspect tavern for a seat, a figure catches your eye. #SPEAKER:Narrator
Ah, you must be the famed adventurer. #SPEAKER:Lady
Here to liberate the people from tyranny?
+ [You could say so]
-Hmph.
Another childish delusion.
If you fancy those fairytales of yours so much, why don't you do me a favor?
I need a book.
A special kind of book.
+[Ok, then]
Wow, really?
You're that gullible?
What are you? A medieval delivery service?
Whatever. I don't care.
Just go to the abandoned house and retrieve my book.
And, whatever you do.
Don't read it by an-
Before she could finish, the innkeeper interrupts her. #SPEAKER:Narrator
Listen, sorceress. We have to discuss about the thing. #SPEAKER:Innkeeper
Can't you wait till I'm done? #SPEAKER:Sorceress
~ SetNextStep(1, _questName) 
Best you take your leave. #SPEAKER:Narrator
-> END
+ [Too busy rn. Bye]
Stand up.#SPEAKER:Narrator
Hey! #SPEAKER:Lady
Refuse to explain.#SPEAKER:Narrator
What are you doing?#SPEAKER:Lady
~ SetNextStep(-1, _questName) //quest was never started
Leave.#SPEAKER:Narrator
Hey, I'm not finished talki-#SPEAKER:Lady
-> END


===Step1===
You approach the area and find it bocked with a magical lock. #SPEAKER:Narrator
How would you tackle this obstacle?
*[Dispel (5 WIS)]
Although drowning in miasma, you should have a spell for this.
~ DoDialogueRoll(5, "WIS")
Mana flows with your incantation. //Pass the result in
{ _rollSuccess :
    [{_rollSuccess}] Click. 
    The lock opens.
    ~ SetNextStep(2, _questName)
    ~ RewardItem("Abandoned House Key")
    The tome should be around here somewhere.
    ->END
    -else:
    [{_rollSuccess}] Lightning strikes the lock.
    Yet nothing happens.
    ->Step1
}
*[Lockpick (5 INT)]
You're a pretty smart guy.
Magic can't beat the long lost art of poking around.
Some fiddling around with hairpins should open it in no time.
~ DoDialogueRoll(5, "INT")
You start assaulting the lock.
{ _rollSuccess :
    [{_rollSuccess}] Click.
    The lock opens and with it, the magic dissipates.
    ~ RewardItem("Abandoned House Key")
    ~ SetNextStep(2, _questName)
    Let's find that tome.
    ->END
    -else:
    [{_rollSuccess}] The lock is not happy.
    It may file a complaint to HR if you keep violating it.
    There has to be another way.
    ->Step1
}
+[Leave]
Her chambers still remain unreachable.
~ SetNextStep(1, _questName)
You'll return to this topic when you have time.
->END


===Step2===
As you scan her chambers for the book, you suddenly trip over something. #SPEAKER:Narrator
It's big, 
bulky, 
veiny 
and 
hideous in sight.
It's a college textbook.
This can't possibly be the book, she's referring to?
+[Keep searching]
-There's physics books, 
english books, 
and cooking books.
Without specific details, you don't know which book to take back.
+[Eenie]
+[Meenie]
+[Minimo]
-You pick up a random book and head back.
No, just kidding.
You summon a Epson Xerox Mechanicus and copy all its contents.
Copyright laws be damned.
~ SetNextStep(3,  _questName)
We can head back to the tavern now.
->END

===Step3===
Oh, you're back. #SPEAKER:Sorceress
I trust that you brought it back with you?
+[Yup]
+[Along with these]
-You slam a mountain of intellectual property violations onto the table #SPEAKER:Narrator
You idiot! #SPEAKER:Sorceress
I told you to get me one book!
And you bring me all this?
How on earth did you think it was feasible to carry all of this!?
I bet you'd jump off a cliff if I told you to.
+[Tips fedora]
+[Yes, m'lady]
-Oh hades, I am talking to a corset sniffer.
Listen, there's no turning back.
I'm putting you down for your own good.
Flames burst from the sorceress' hands as she prepares herself for battle. #SPEAKER:Narrator
Her fury will be unrelenting should you commit to battle.
    +[Fight]
    You scram to find a weapon from any of the desks.
    Yet the best you can manage are some rolled up rolls of the photocopied documents.
    20 sheets of false tax declarations are all that stands between you and furious flames.
    Oh ho, you're approaching me of instead of running away? #SPEAKER:Sorceress
    I can't beat the shit out of you without getting closer. #SPEAKER:You 
    ~SetNextStep(4, _questName)
    The battle begins #SPEAKER:Narrator
    //startFIghtFunc 
    ->END
    
    +[Run] -> Run

=== Run ===
    Balls of fire slam around you as you dash for the door.#SPEAKER:Narrator
    Soon the fires start to spread amidst her careless aim.
    Guests start screaming at the growing fire.
    [Sips coffee] This is fine #SPEAKER:Dog
    Smoke fills the air yet you still keep running.#SPEAKER:Narrator
    Coughing... you somehow come out the otherside unscathe.
    Well maybe except for your dignity.
    ~SetNextStep(4, _questName)
    As the adrenaline wears off.
    Standing here you realize that
    Just like me this was a complete
    +[Filler quest]
    +[Waste of time]
    -At least you feel stronger from the exercise.
    ->END