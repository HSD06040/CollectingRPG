using System.Collections.Generic;

// 7가지 임시 이벤트 데이터
public static class StageDialogEvents
{
    public static DialogEvent[] GetDefault7Events()
    {
        return new DialogEvent[]
        {
            // 1001: 임시 이벤트 1
            new DialogEvent
            {
                eventId = "1001",
                dialogText = "임시 이벤트 1 - 추가 예정",
                choices = new List<DialogChoice>
                {
                    new DialogChoice
                    {
                        choiceText = "선택지 1",
                        results = new List<DialogResult>
                        {
                            new DialogResult { resultId = "1-1", resultText = "결과 1-1", rewardDescription = "TODO: 보상 1" },
                            new DialogResult { resultId = "1-2", resultText = "결과 1-2", rewardDescription = "TODO: 보상 2" }
                        }
                    },
                    new DialogChoice
                    {
                        choiceText = "선택지 2",
                        results = new List<DialogResult>
                        {
                            new DialogResult { resultId = "2-1", resultText = "결과 2-1", rewardDescription = "TODO: 보상 없음" }
                        }
                    }
                }
            },
            
            // 1002: 임시 이벤트 2
            new DialogEvent
            {
                eventId = "1002", 
                dialogText = "임시 이벤트 2 - 추가 예정",
                choices = new List<DialogChoice>
                {
                    new DialogChoice
                    {
                        choiceText = "선택지 1",
                        results = new List<DialogResult>
                        {
                            new DialogResult { resultId = "1-1", resultText = "결과 1-1", rewardDescription = "TODO: 보상 1" },
                            new DialogResult { resultId = "1-2", resultText = "결과 1-2", rewardDescription = "TODO: 보상 2" }
                        }
                    },
                    new DialogChoice
                    {
                        choiceText = "선택지 2",
                        results = new List<DialogResult>
                        {
                            new DialogResult { resultId = "2-1", resultText = "결과 2-1", rewardDescription = "TODO: 보상 없음" }
                        }
                    }
                }
            },
            
            // 1003: 임시 이벤트 3
            new DialogEvent
            {
                eventId = "1003",
                dialogText = "임시 이벤트 3 - 추가 예정",
                choices = new List<DialogChoice>
                {
                    new DialogChoice
                    {
                        choiceText = "선택지 1",
                        results = new List<DialogResult>
                        {
                            new DialogResult { resultId = "1-1", resultText = "결과 1-1", rewardDescription = "TODO: 보상 1" },
                            new DialogResult { resultId = "1-2", resultText = "결과 1-2", rewardDescription = "TODO: 보상 2" }
                        }
                    },
                    new DialogChoice
                    {
                        choiceText = "선택지 2",
                        results = new List<DialogResult>
                        {
                            new DialogResult { resultId = "2-1", resultText = "결과 2-1", rewardDescription = "TODO: 보상 없음" }
                        }
                    }
                }
            },
            
            // 1004: 임시 이벤트 4
            new DialogEvent
            {
                eventId = "1004",
                dialogText = "임시 이벤트 4 - 추가 예정",
                choices = new List<DialogChoice>
                {
                    new DialogChoice
                    {
                        choiceText = "선택지 1",
                        results = new List<DialogResult>
                        {
                            new DialogResult { resultId = "1-1", resultText = "결과 1-1", rewardDescription = "TODO: 보상 1" }
                        }
                    },
                    new DialogChoice
                    {
                        choiceText = "선택지 2", 
                        results = new List<DialogResult>
                        {
                            new DialogResult { resultId = "2-1", resultText = "결과 2-1", rewardDescription = "TODO: 보상 2" }
                        }
                    }
                }
            },
            
            // 1005: 임시 이벤트 5
            new DialogEvent
            {
                eventId = "1005",
                dialogText = "임시 이벤트 5 - 추가 예정",
                choices = new List<DialogChoice>
                {
                    new DialogChoice 
                    { 
                        choiceText = "선택지 1", 
                        results = new List<DialogResult> 
                        { 
                            new DialogResult { resultId = "1-1", resultText = "결과 1-1", rewardDescription = "TODO: 보상 1" } 
                        } 
                    },
                    new DialogChoice 
                    { 
                        choiceText = "선택지 2", 
                        results = new List<DialogResult> 
                        { 
                            new DialogResult { resultId = "2-1", resultText = "결과 2-1", rewardDescription = "TODO: 보상 2" } 
                        } 
                    }
                }
            },
            
            // 1006: 임시 이벤트 6
            new DialogEvent
            {
                eventId = "1006", 
                dialogText = "임시 이벤트 6 - 추가 예정",
                choices = new List<DialogChoice>
                {
                    new DialogChoice 
                    { 
                        choiceText = "선택지 1", 
                        results = new List<DialogResult> 
                        { 
                            new DialogResult { resultId = "1-1", resultText = "결과 1-1", rewardDescription = "TODO: 보상 1" } 
                        } 
                    },
                    new DialogChoice 
                    { 
                        choiceText = "선택지 2", 
                        results = new List<DialogResult> 
                        { 
                            new DialogResult { resultId = "2-1", resultText = "결과 2-1", rewardDescription = "TODO: 보상 2" } 
                        } 
                    }
                }
            },
            
            // 1007: 임시 이벤트 7
            new DialogEvent
            {
                eventId = "1007",
                dialogText = "임시 이벤트 7 - 추가 예정", 
                choices = new List<DialogChoice>
                {
                    new DialogChoice 
                    { 
                        choiceText = "선택지 1", 
                        results = new List<DialogResult> 
                        { 
                            new DialogResult { resultId = "1-1", resultText = "결과 1-1", rewardDescription = "TODO: 보상 1" } 
                        } 
                    },
                    new DialogChoice 
                    { 
                        choiceText = "선택지 2", 
                        results = new List<DialogResult> 
                        { 
                            new DialogResult { resultId = "2-1", resultText = "결과 2-1", rewardDescription = "TODO: 보상 2" } 
                        } 
                    }
                }
            }
        };
    }
}