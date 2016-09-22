import { fromJS } from 'immutable';

const initialState = {
'active':'',
'guilds': [
  {
    "guild": "guild 1",
    "groups": []
  },
  {
    "guild": "guild 2",
    "groups": [
      {
        "tutors": [
          "tutor 1",
          "tutor 2"
        ],
        "students": [
          {
            "name": "student example1"
          },
          {
            "name": "student example2"
          }
        ]
      }
    ]
  }
]

export const immutableState = fromJS(initialState);