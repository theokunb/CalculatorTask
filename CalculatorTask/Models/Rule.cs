using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorTask.Models
{
    public class Rule
    {
        private delegate bool AllowFunction(Type type);

        private readonly List<Token> tokens;
        private readonly Dictionary<Type, AllowFunction> followSets;
        private readonly Stack<Type> stackBktOpen;
        private Type lastToken;
        private bool hasPoint;

        public Rule(List<Token> tokenList)
        {
            followSets = new Dictionary<Type, AllowFunction>();
            tokens = tokenList;
            stackBktOpen = new Stack<Type>();
            hasPoint = false;
            CreateRules();
        }

        private void CreateRules()
        {
            followSets.Add(typeof(Number), FollowNumber);
            followSets.Add(typeof(LeftBkt), FollowLeftBkt);
            followSets.Add(typeof(RightBkt), FollowRightBkt);
            followSets.Add(typeof(AddOperator), FollowOperator);
            followSets.Add(typeof(SubOperator), FollowOperator);
            followSets.Add(typeof(MulOperator), FollowOperator);
            followSets.Add(typeof(DivOperator), FollowOperator);
            followSets.Add(typeof(Point), FollowPoint);
        }

        private bool FollowNumber(Type type)
        {
            Type[] passTypes;

            if (stackBktOpen.Count > 0)
                passTypes = new Type[] { typeof(Number), typeof(Point), typeof(AddOperator), typeof(SubOperator), typeof(MulOperator), typeof(DivOperator), typeof(RightBkt) };
            else
                passTypes = new Type[] { typeof(Number), typeof(Point), typeof(AddOperator), typeof(SubOperator), typeof(MulOperator), typeof(DivOperator) };

            if (passTypes.Contains(type))
            {
                if (type.Equals(typeof(Point)))
                {
                    if (hasPoint)
                        return false;
                    else
                        hasPoint = true;
                }
                else if (!type.Equals(typeof(Number)))
                    hasPoint = false;

                lastToken = type;
                CheckRightBkt(type);
                return true;
            }
            else
                return false;

        }

        private bool FollowPoint(Type type)
        {
            Type[] passType = { typeof(Number) };

            if (passType.Contains(type))
                return true;

            return false;
        }

        private bool FollowLeftBkt(Type type)
        {
            Type[] passTypes = { typeof(Number), typeof(LeftBkt) };

            if (passTypes.Contains(type))
            {
                CheckLeftBkt(type);
                lastToken = type;
                return true;
            }
            else
                return false;
        }

        private bool FollowRightBkt(Type type)
        {
            Type[] passTypes;

            if (stackBktOpen.Count > 0)
                passTypes = new Type[] { typeof(AddOperator), typeof(SubOperator), typeof(MulOperator), typeof(DivOperator), typeof(RightBkt) };
            else
                passTypes = new Type[] { typeof(AddOperator), typeof(SubOperator), typeof(MulOperator), typeof(DivOperator) };

            if (passTypes.Contains(type))
            {
                lastToken = type;
                CheckRightBkt(type);
                return true;
            }
            else
                return false;
        }

        private bool FollowOperator(Type type)
        {
            Type[] passTypes = { typeof(Number), typeof(LeftBkt) };

            if (passTypes.Contains(type))
            {
                CheckLeftBkt(type);
                lastToken = type;
                return true;
            }
            else
                return false;
        }

        public bool AllowFollow(char lastChar, string follow)
        {
            var typeOfLast = tokens.Find(element => element == lastChar.ToString());
            var typeOfFollow = tokens.Find(element => element == follow);

            return followSets[typeOfLast.GetType()].Invoke(typeOfFollow.GetType());
        }

        public bool AllowFirst(string first)
        {
            Type[] passTypes = { typeof(LeftBkt), typeof(Number) };
            var typeOfFirst = tokens.Find(element => element == first);
            var res = passTypes.Contains(typeOfFirst.GetType());
            CheckLeftBkt(typeOfFirst.GetType());

            return res;
        }

        public bool IsLineValidToSolve()
        {
            Type[] invalidLastTokens = { typeof(AddOperator), typeof(SubOperator), typeof(MulOperator), typeof(DivOperator), typeof(LeftBkt) };

            if (stackBktOpen.Count == 0 && !invalidLastTokens.Contains(lastToken))
                return true;

            return false;
        }

        private void CheckRightBkt(Type type)
        {
            if (type.Equals(typeof(RightBkt)))
            {
                stackBktOpen.Pop();
            }
        }
        private void CheckLeftBkt(Type type)
        {
            if (type.Equals(typeof(LeftBkt)))
            {
                stackBktOpen.Push(type);
            }
        }
    }
}
