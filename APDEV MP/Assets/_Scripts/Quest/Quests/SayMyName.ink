EXTERNAL SetNextStep(nNextStep, strQuestName)
EXTERNAL DoDialogueRoll(nStatRequired, strStatType)

CONST _questName = "Say My Name"
VAR _questStep = 0
VAR _rollSuccess = false

{_questStep :
- 0: -> Step0
- 1: -> Step1
- 2: -> Step2
- 3: -> Step3
- 4: -> Step4
}


===Step0===
Hello again, dear. #SPEAKER:Grandma
I sometimes worry about my grandchild.
Can you help bring the kiddo home?
+ [Sure, where'd they go?]
~ SetNextStep(1, _questName) 
He went out playing alone.
Maybe you can ask the other kids around.
Hurry on now.
-> END

+ [Not, right now.]
~ SetNextStep(-1, _questName) //quest was never started
It's getting quite late though.
Please help when you have some spare time.
-> END

===Step1===
Hey, traveller. #SPEAKER:Kid
Are you here to kill the Demon Lord?
+ [You betcha]
+ [I'm gonna rip their guts out]
- Hell ya!
Some day, I wanna be a genocidal maniac like you too.
Ripping and tearing all the way!
From the river to the sea, we'll be free of the demons!
+[Umm...]
+[Okay...]
- If there's anything I can do to help, let me know!
+[Help me find Granny's kid]
~ SetNextStep(2, _questName)
Why, is he a demon!?
I knew it!
I saw him playing by that big tree over there.
Bet he's still there right now.
You go first. 
I'll get the chainsaw.
-> END

===Step2===
You see a child playing on top of the tree. #SPEAKER:Narrator
What's your plan?

* [Shake the tree CON(10)]
    You furiously shake the tree.
    Waaaaaaaaaah!!!! #SPEAKER:Tree Kid
    Why's the tree shaking!??
    I don't want to die!
    The kid desperately clings to random objects. #SPEAKER:Narrator
    Each shake starts hurling twigs and branches all around until... 
    A roadroller-sized pinecone heads toward you.
    ~ DoDialogueRoll(10, "CON")
    You brace for impact.
    { _rollSuccess :
        [{_rollSuccess}] The pinecone smashes onto your head. 
            ~ SetNextStep(3, _questName)
            Yet it barely phases you.
            They don't call you the "Nut cracker" without reason.
            But still...
            Maybe talking would be a better option. 
            ->END
            -else:
            [{_rollSuccess}] Wryyyyyyyyyyyy!!!
            The pinecone smashes onto your head.
            Your vision is blurred with vertigo.
            At least the pinecone, makes a nice hat.
            ->Step2
    }    
* [Climb the tree DEX(10)]
    You begin reaching for branches and pulling yourself up.
    Years of training has blessed you with the "Pull up blessing".
    A blessing only granted by the holiest of temples...
    The Lord's Holy Gym.
    As you can continue climbing, the kid notices your figure.
    Waaaaaaaaaaaaaah!!! #SPEAKER:Tree Kid
    It's a gorilla!
    Losing the element of surprise, you continue to climb faster. #SPEAKER:Narrator
    ~ DoDialogueRoll(10, "DEX")
    I am Allah's strongest warrior!!! #SPEAKER: You
    {_rollSuccess:
        [{_rollSuccess}] You blaze through the tree at impossible speeds. #SPEAKER:Narrator
            ~ SetNextStep(3, _questName)
            But then suddenly, rocks are hurled at you.
            George, get the shotty! #SPEAKER:Hillybilly
            It's one of 'em gymrats! 
            Whoooooehhhhhhhh! We've got dinner.
            It'll be a juicy one tonight!
            Terrified at the prospect of becoming a protein shake, you retreat downwards.
            Maybe talking would a better option.
            ->END
        -else:
        [{_rollSuccess}] But alas, you've spent your energy. #SPEAKER:Narrator
            One by one, your grip betrays you.
            Each finger slips against your wishes until you fall from the tree.
            A crashing thump rocks the treeline.
            Perhaps it's time to reconsider.
            ->Step2
    }
+ [Leave for now]
    ~ SetNextStep(2, _questName)
    The kid can continue playing.
    You have more pressing matters to attend to.
    -> END
    
=== Step3 ===
Hey, what do you think you're doing!? #SPEAKER:Tree Kid
I could fall from this height!
+ [Grandma's calling] 
+ [You need to go home] 
- No, I don't want to!
I heard Grandma yelling my name.
I'm scared.
What if she feeds me to the demons?
+[There's nothing to be scared of]
+[Your grandma is harmless]
- I don't believe you and I don't even know who you are.
+[I'm the hero] 
+[I won't hurt you] 
+[Now, tell me your name]
- My name?
+[Yes, your name]
- I'm Pharmaceutical Greed #SPEAKER:Kid named Pharmaceutical Greed
 
+[Hi, I'm Biggus Dickus]
- Hey, stop making fun of my name!
I didn't ask for any of this.
Even the other kids think I'm a demon spy.
I just want to be left alone.
+[I see]
+[Leave]
- Hey, you promised you'd help!
Where are you going!?
+[To grandma]
- Nooooooooooo.
Grandma will kill me!
I don't want to die.
~ SetNextStep(4, _questName)
Not like this.
-> END

=== Step4 ===
Dear, you're back. #SPEAKER: Grandma
Did you find where my grandchild is at?
+ [Yes]
- Oh thank goodness.
I thought he hurt himself.
+ [Rowdy one, isn't he?]
-Oh, you know it.
Ah sometimes the sandals just aren't enough.
Pharmaceutical Greed really has quite the imagination.
+[Granny, why's he named...]
+[Pharmaceutical Greed?]
-Darling. Such a silly question.
It's because his mother loves medicine
and money.
+[Thanks, Granny]
No problem, Free Laborer.
Ahem.
Oh, the trouble he's caused us all.
I still want to see him grow up one day to become Big Pharma.
Heavens know if I'll live to that day.
Oh, sorry dear.
Here I go babbling again.
Don't let me keep you any longer.
~ SetNextStep(5, _questName)
You can go now.
-> END