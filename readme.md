# SuperDraft Coding Exercise (Backend)
---

## DFS, what is it?
---
DFS, or daily fantasy sports, is a game of skill in which competitors select a set of players from real life sporting events to construct a lineup. Players in each lineup then accrue points based on statistics accrued in a real-life sporting event. Contests are held on a daily basis in which competitors submit one or more lineups in hopes of scoring the highest number of fantasy points in the contest. The highest scoring lineups in a contest then win cash prizes.

## Goal of the exercise
---
The goal of this coding exercise is to take a set of un-scored lineups from a contest and perform the following:

1. Apply points to each player in each lineup
2. Rank each lineup in the contest by total number of fantasy points
3. Apply the total winnings to each lineup that finished high enough to receive a cash prize. 
4. Submit the scored lineups back to the API.

## API
---

When accessing the API endpoints, you'll need to supply an API key header with the name `SD-api-key` with the value being the API key provided to you by SuperDraft. 

```json
SD-api-key: {YOUR_API_KEY}
```

For the purpose of this exercise, you'll be using ContestId **186066** for all API requests that require a contest id. 

### Lineups
---
To acquire the lineups for the contest you'll be scoring, you'll need to make a request to the [Get Contest Lineups](https://api-candidates.staging.superdraft.io/swagger/index.html#/Lineups/Lineups_GetContestLineups) endpoint.

### Contest Prizes
---
To acquire the contest prizes, you'll need to make a request to the [Get Contest](https://api-candidates.staging.superdraft.io/swagger/index.html#/Contests/Contests_GetContest) endpoint. On the returned contest object, there will be an array of prizes.

Each prize will contain an `amount` that indicates the dollar amount of the prize to be granted. Additionally, each prize will contain `from` and `to` properties that indicate the ranked positions that will receive the prize. 

For example, a prize package that looks as follows:

```json
{
    "position": 11,
    "from": 11,
    "to": 15,
    "amount": 80,
    "packageTotal": 400
}
```

indicates that lineups that finish in the 11th - 15th position will receive a prize of $80 USD.

### Player Scores
---
To acquire player scores for the contest, you'll need to make an API request to the [Get Contest Player Scores](https://api-candidates.staging.superdraft.io/swagger/index.html#/PlayerScores/PlayerScores_GetContest) endpoint. 

This endpoint returns an array of objects that consist of `playerId` and `score` properties. The `playerId` is the unique identifier of the player who scored the points, and the `score` is the number of points that player scored. 

```json
[
    {
        "playerId": 845564,
        "score": 48.25
    },
    {
        "playerId": 1061507,
        "score": 35.75
    },
    ...
]
```

### Scoring Lineups
---

In order to score a lineup, you'll need to find the associated player score for the player in question, and multiply that score by the lineup player's `multiplier`. 

For example, if we were attempting to score `playerId` `845564` (Devin Booker)

```json
{
    "playerId": 845564,
    "multiplier": 1.45,
    "firstName": "Devin",
    "lastName": "Booker",
    "score": 0,
    "team": "PHO",
    "position": "SG"
}
```

We'd need to lookup his player score, and multiply that score by his `1.45` multiplier. The resulting number would then be applied to Devin Booker's `score` property. 

*For example, the correct score for Devin Booker would be*
```
1.45 x 48.25 = 69.96 (rounded to 2 decimal places)
```

The total score for a lineup is the sum of the scores of all the players in that lineup. Once the total lineup score is computed, you'll want to update the `score` property on the lineup.

### Ranking Lineups
---
Once all lineups have been scored, you'll need to rank the lineups from highest to lowest score. When applying lineup ranks, you'll want to do so using the [Standard Competition Ranking Strategy](https://en.wikipedia.org/wiki/Ranking#:~:text=In%20competition%20ranking%2C%20items%20that,left%20in%20the%20ranking%20numbers.&text=Equivalently%2C%20each%20item's%20ranking%20number,of%20items%20ranked%20above%20it.). 

Once you've completed ranking the lineups, you'll want to update the `position` property on each lineup to reflect it's rank within the contest. 

### Prize Assignment
---
When assigning prizes to lineups, you'll want to follow the specifications outlined in the `prizes` array of the `contest` object. In addition, you'll need to take ties into consideration when applying prizes across multiple prize packages.

For example, if there are two lineups tied for 2nd place, each 2nd place lineup would receive prizes equal to the `2nd place prize` plus the `3rd place prize` divided by 2.

```json
{
    "position": 2,
    "from": 2,
    "to": 2,
    "amount": 1000,
    "packageTotal": 1000
},
{
    "position": 3,
    "from": 3,
    "to": 3,
    "amount": 700,
    "packageTotal": 700
}
```

As a result, the two lineups tied for 2nd place would each receive a total prize of `$850.00` ((1000 + 700) / 2). 


### Submission
---
To submit your scored lineups, make a request to the [Submit Scored Lineups](https://api-candidates.staging.superdraft.io/swagger/index.html#/Lineups/Lineups_SubmitScoredLineups) endpoint.

When submitting your scored lineups, it is expected that:

*Each lineup has the following properties populated:*
* position
* totalWinnings
* score

*Each player in each lineup has the following properties populated:*
* score

