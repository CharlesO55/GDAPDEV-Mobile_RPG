EXTERNAL SetNextStep(nNextStep, strQuestName)
EXTERNAL DoDialogueRoll(nStatRequired, strStatType)
EXTERNAL RewardItem(strItemName)
EXTERNAL StartBattle()

CONST _questName = "A Ritual Gone Wrong"
VAR _questStep = 0
VAR _rollSuccess = false

{_questStep :
- 0: -> Step0
- 1: -> Step1
- 2: -> Step2
- 3: -> Step3
}


===Step0===
As you're walking, a peculiar house appears. #SPEAKER:Narrator
Its windows are blasting rays of blinding light.
You swear you can hear CaramelDansen playing from inside.
You walk past to see a man standing outside the house.
Hey, you. #SPEAKER:Man
I need some help over here.
My house was overran by demons.
Luckily I managed to escape just in time and lock the door.
+ [Sounds scary man]
+ [Burn it down then]
- Are you crazy!?
That's my home you're talking about.
+ [The Demon King]
+ [Provides insurance claims]
- Do you take me for a conman?
I'll have you know that I'm Vergil the Great. #SPEAKER:Vergil
One of the previous heros who overthrew the Demon King.
+ [You can handle it then]
+ [Good luck]
- Ah wait. Wait!
I could still use your help.
+[Help]
Great, I knew I could count on you.
Listen, here's the key.
~RewardItem("Vergil House Key")
You go in the front door and start morbing all over the place.
And I'll umm...
I'll take the back door.
Got it?
~SetNextStep(1, _questName)
Great! Now get rid of those demons.
->END
+[Leave]
~ SetNextStep(-1, _questName) 
You take your leave as the demons turn this house into a moshpit. #SPEAKER:Narrator
->END


===Step1===
You succesfully enter the house and find the demons have made a mess of the place. #SPEAKER:Narrator
The demons notice your entrance and briefly pause.
Hey, bro. You here for the party to? #SPEAKER:Demon
+[Yes]
->Party
+[Leave]
Oh, shame.
~SetNextStep(1, _questName)
Feel free to crash here when you need a place.
->END

===Party===
Nice. #SPEAKER:Demon
Drinks are to the left and pizza is to the right.
Vergil is the bomb when it comes to these parties!
+[Vergil planned this?]
- Ya, see those summoning circles.
Dude scribbled all over the place and filled it with enough mana to summon an army.
+[Why'd he summon]
+[Demons]
- Don't ask me bro. I'm not the Legendary Necromancer of the Crypt.
If he wanted to party with us demons, he could've just set fliers all over the city.
Every demon in town couldn't resist clubbing.
Even the incubbi waiters would take a day off from Femboy Hooters.
Anyways, you do you.
Party's on full blast and those summoning circles will keep bringing in more guests.
Ciao, bro.
~SetNextStep(2, _questName)
We should probably stop those summoning circles. #SPEAKER:Narrator
->END

===Step2===
You begin investigating the summoning circle responsible for this. #SPEAKER:Narrator
Before you can finish, it suddenly glows immensely.
You step back to avoid being caught in it.
Everybody, run! #SPEAKER:Demons
The Key demon is here! 
A demonic arm soon emerges from the portal. #SPEAKER:Narrator
Slowly it drags drags itself out.
+[What's the key demon?]
-It's the demonic party pooper! #SPEAKER:Demon
Like a vampire that sucks all your rizz!
Whatever this demon is it seems to be a powerful opponent.#SPEAKER:Narrator
More than that it has a key on its neck.
It may be prove useful to take it.
RIZZLER! Where is it!?#SPEAKER:Key Demon
+[Fight]
-The demon howls an unholy screech as the battle begins.#SPEAKER:Narrator
Let's defeat the Key demon and get that key.
You lunge into action with your sword flailing left and right.
Just as you make contact, the demon parries and sends you flying.
Woah, why so hostile? #SPEAKER:Key Demon
I'm just here to party.
+[Wait, you also]
+[want to party?]
- Duh.
Virgil, sent me an invite.
He seemed really happy and giggity. Something about finally cleaning out.
He said I could make a lot of friends here.
But everyone already left.
Why's everyone gotta be so rude?
Don't they know I'm demon too?
The key demon begins to break down in tears. #SPEAKER:Narrator
It's honestly kinda sad.
+[COMFORT (0 CHA)]
-You try your best to comfort the Key demon. #SPEAKER:Narrator
~DoDialogueRoll(0, "CHA")
You try to start a conversation to get him back in shape.
{ _rollSuccess :
    [{_rollSuccess}] How's the weather today?
    -else:
    [{_rollSuccess}] Did you really fail?
}
No matter how abysmal you are with words.
+[It's alright]
+[There's still more parties]
-You don't get it. #SPEAKER:Key Demon
Every party I go to always ends like this!
I just want to have fun for once.
+[You can always]
+[have fun]
+[by yoursellf]
-Ya, that's what everyone says.
But I got work.
I can't just go ball'n whenever I want.
+[I can take your place]
-No, I shouldn't.
My work is important.
+[It's just one shift]
-Hmmmmm...
I mean you just stand at a gate all day.
Fine.
Here take this.
~RewardItem("Keep Gate Key")
It's a key to the castle gate.
+[What's my job?]
-You're now a key demon.
Make sure that no one EVER takes this key from you.
Or That's what the Demon King told me.
Anyways, I'm off to the next party in town.
+[Have fun]
-Vergil will probably be waiting.#SPEAKER:Narrator
~SetNextStep(3, _questName)
We should return.
->END

===Step3===
You exit the house to see Vergil hitting the nae nae like a skibbidi toilet #SPEAKER:Narrator
YES! They're gone! #SPEAKER:Vergil
Finally, I can complete the ritual.
Vergil barely notices you and begins enchanting the mana stones around his home.#SPEAKER:Narrator
The summoning circles light up once more.
Lorem ipsum. Lorem ipsum. Lorem ipsum.#SPEAKER:Vergil
Rise undead of Holden and heed my call.
Unleash the rage rooted in your bon--
YOOOOOOOOO.#SPEAKER:Demons
It's another rave fest from Vergil.
Just then a variety of demons begin storming into Vergil's home once more.#SPEAKER:Narrator
Mina-saaaaaaaaaaaaaaan.#SPEAKER:Youkai
This incubussy wants to get topped tonight.#SPEAKER:Incubus
Mabuhay. #SPEAKER:Aswang
Noooooooooooooo. Get OuUttTtTT! #SPEAKER:Vergil
~SetNextStep(4, _questName)
Heheheheeeeeeeee #SPEAKER:Banshee
->END