using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int[] numbers;
    private Text[] _2048Units;
    private bool gameOver;
    public GameObject overPanel;
    public GameObject scoreDisplay;
    public GameObject heighestScoreDisplay;
    private int score;
    public List<Color> colors;


    // Use this for initialization
    void Start()
    {
        numbers = new int[16];
        _2048Units = transform.GetComponentsInChildren<Text>();
        if (_2048Units.Length != 16)
        {
            Debug.LogError("可显示 text 组件数量不足");
            Application.Quit();
        }

        Init();
    }

    private int Column
    {
        get { return 4; }
    }

    private int Row
    {
        get { return 4; }
    }


    public void Init()
    {
        System.Random r = new System.Random();

        for (int i = 0; i < numbers.Length; i++)
        {
            if (r.Next(10) > 6)
            {
                numbers[i] = r.Next(100) > 30 ? 2 : 4;
            }
            else
            {
                numbers[i] = 0;
            }
        }

        gameOver = false;
        score = 0;
        RefreshDisplay();

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            Vector2 input = GetInput();
            if (input.x != 0 || input.y != 0)
            {
                MoveUnit(input);
            }
        }
    }

    Vector2 GetInput()
    {
        bool right = Input.GetKeyDown(KeyCode.D);
        bool left = Input.GetKeyDown(KeyCode.A);
        bool up = Input.GetKeyDown(KeyCode.W);
        bool down = Input.GetKeyDown(KeyCode.S);
        Vector2 input = new Vector2();
        if (right)
        {
            input.x = 1;
        }
        else if (left)
        {
            input.x = -1;
        }
        else
        {
            input.x = 0;
        }

        if (up)
        {
            input.y = 1;
        }
        else if (down)
        {
            input.y = -1;
        }
        else
        {
            input.y = 0;
        }

        return input;
    }

    void MoveUnit(Vector2 axis)
    {
        int addScore = 0;
        //横向移动
        if (axis.x != 0)
        {
            for (int i = 0; i < Row; i++)
            {
                //获取行
                int[] row = new int[]
                {
                    numbers[i * Column],
                    numbers[i * Column + 1],
                    numbers[i * Column + 2],
                    numbers[i * Column + 3]
                };
                //向右移动
                if (axis.x == 1)
                {
                    for (int j = 2; j >= 0; j--)
                    {
                        if (row[j + 1] == 0)
                        {
                            row[j + 1] = row[j];
                            row[j] = 0;
                        }
                        //合并
                        else
                        {
                            if (row[j + 1] == row[j])
                            {
                                addScore += row[j + 1];
                                row[j + 1] *= 2;
                                row[j] = 0;
                            }
                        }
                    }
                }
                //向左移动
                else if (axis.x == -1)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        if (row[j - 1] == 0)
                        {
                            row[j - 1] = row[j];
                            row[j] = 0;
                        }
                        else
                        {
                            if (row[j - 1] == row[j])
                            {
                                addScore += row[j - 1];

                                row[j - 1] *= 2;
                                row[j] = 0;
                            }
                        }
                    }
                }

                //赋值
                numbers[i * Column] = row[0];
                numbers[i * Column + 1] = row[1];
                numbers[i * Column + 2] = row[2];
                numbers[i * Column + 3] = row[3];
            }

            AddNumber();
        }
        //纵向移动
        else if (axis.y != 0)
        {
            for (int i = 0; i < Column; i++)
            {
                //获取列
                int[] column = new int[]
                {
                    numbers[i + Row * 0],
                    numbers[i + Row * 1],
                    numbers[i + Row * 2],
                    numbers[i + Row * 3]
                };
                //向上移动
                if (axis.y == 1)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        if (column[j - 1] == 0)
                        {
                            column[j - 1] = column[j];
                            column[j] = 0;
                        }
                        else
                        {
                            if (column[j - 1] == column[j])
                            {
                                addScore += column[j - 1];

                                column[j - 1] *= 2;
                                column[j] = 0;
                            }
                        }
                    }
                }
                //向下移动
                else if (axis.y == -1)
                {
                    for (int j = 2; j >= 0; j--)
                    {
                        if (column[j + 1] == 0)
                        {
                            column[j + 1] = column[j];
                            column[j] = 0;
                        }
                        else
                        {
                            if (column[j + 1] == column[j])
                            {
                                addScore += column[j + 1];

                                column[j + 1] *= 2;
                                column[j] = 0;
                            }
                        }
                    }
                }

                numbers[i] = column[0];
                numbers[i + Row] = column[1];
                numbers[i + Row * 2] = column[2];
                numbers[i + Row * 3] = column[3];
            }

            AddNumber();
        }

        AddScore(addScore);
        RefreshDisplay();
        gameOver = IsGameOver();
        if (gameOver)
        {
            print(gameOver);
            overPanel.SetActive(true);
        }
    }

    void AddNumber()
    {
        int zeroCount = 0;
        int index = -1;
        for (int i = 0; i < numbers.Length; i++)
        {
            if (numbers[i] == 0)
            {
                zeroCount += 1;
                index = i;
            }
        }

        if (zeroCount == 1)
        {
            numbers[index] = 2;
        }
        for (int i = 0; i < numbers.Length; i++)
        {
            System.Random r = new System.Random(Guid.NewGuid().GetHashCode());
            int rn = r.Next(10);
            if (numbers[i] == 0)
            {
                if (rn > 3 && rn < 8)
                {
                    numbers[i] = 2;
                    break;
                }
                else if (rn <= 3)
                {
                    numbers[i] = 4;
                    break;
                }
                else
                {
                    numbers[i] = 0;
                }
            }
        }
    }

    void RefreshDisplay()
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            Color color = Color.white;
            switch (numbers[i])
            {
                case 0:
                    color = colors[0];
                    break;
                case 2:
                    color = colors[1];
                    break;
                case 4:
                    color = colors[2];
                    break;
                case 8:
                    color = colors[3];
                    break;
                case 16:
                    color = colors[4];
                    break;
                case 32:
                    color = colors[5];
                    break;
                case 64:
                    color = colors[6];
                    break;
                case 128:
                    color = colors[7];
                    break;
                case 256:
                    color = colors[8];
                    break;
                default:
                    color = colors[9];
                    break;
            }

            string text = numbers[i] == 0 ? "" : numbers[i].ToString();
            _2048Units[i].text = text;
            _2048Units[i].transform.parent.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 255);

        }
        scoreDisplay.GetComponent<Text>().text = score.ToString();
        heighestScoreDisplay.GetComponent<Text>().text = PlayerPrefs.GetInt("HeighestScore").ToString();
    }

    void AddScore(int addScore)
    {
        score += addScore;
        int heighestScore = PlayerPrefs.GetInt("HeighestScore");
        if (heighestScore < score)
        {
            PlayerPrefs.SetInt("HeighestScore",score);
        }
    }

    bool IsGameOver()
    {
        if (numbers.Contains(0))
        {
            return false;
        }

        for (int i = 0; i < numbers.Length; i++)
        {
            if ((i + 1) % 4 != 0)
            {
                if (numbers[i] == numbers[i + 1])
                {
                    return false;
                }
            }

            if (i < numbers.Length - Column)
            {
                if (numbers[i] == numbers[i + Row])
                {
                    return false;
                }
            }
        }

        return true;
    }

}