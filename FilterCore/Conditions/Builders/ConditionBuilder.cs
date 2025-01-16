using FilterCore.Conditions.Impl;

namespace FilterCore.Conditions.Builders;

public interface IConditionBuilder
{
    public IEnumerable<FilteringCondition> Apply();
}

public interface IBaseTypeConditionBuilder : IConditionBuilder
{
    public IBaseTypeConditionBuilder Include(params string[] baseName);
    public IBaseTypeConditionBuilder Exclude(params string[] baseName);

    public ConditionBuilder And();
}
public class ConditionBuilder : IConditionBuilder
{
    private List<FilteringCondition> _conditions = [];

    private ConditionBuilder()
    {

    }

    public static ConditionBuilder Start() => new ConditionBuilder();

    public IBaseTypeConditionBuilder FilterByTypes(params string[] types)
    {
        return new BaseTypeBuilder(this, types);
    }

    public ConditionBuilder FilterByStackSize(int count)
    {
        _conditions.Add(new StackSizeCondition(count));
        return this;
    }

    public IEnumerable<FilteringCondition> Apply()
    {
        return _conditions;
    }

    private abstract class InnerBuilder : IConditionBuilder
    {
        protected ConditionBuilder _parent;

        public InnerBuilder(ConditionBuilder parent)
        {
            _parent = parent;
        }

        public virtual IEnumerable<FilteringCondition> Apply()
        {
            return _parent.Apply();
        }
    }

    private class BaseTypeBuilder : InnerBuilder,  IBaseTypeConditionBuilder
    {
        private readonly HashSet<string> _baseTypes = [];

        
        public BaseTypeBuilder(ConditionBuilder parent, params string[] types) : base(parent)
        {
            foreach(var item in types)
            {
                _baseTypes.Add(item);
            }
        }

        

        ConditionBuilder IBaseTypeConditionBuilder.And()
        {
            _parent._conditions.Add(new BaseTypeCondition(_baseTypes));
            return _parent;
        }
        IBaseTypeConditionBuilder IBaseTypeConditionBuilder.Exclude(params string[] types)
        {
            foreach (var item in types)
            {
                _baseTypes.Add(item);
            }
            return this;
        }
        IBaseTypeConditionBuilder IBaseTypeConditionBuilder.Include(params string[] types)
        {
            foreach (var item in types)
            {
                _baseTypes.Add(item);
            }
            return this;
        }

        public override IEnumerable<FilteringCondition> Apply()
        {
            _parent._conditions.AddRange(new BaseTypeCondition(_baseTypes));
            return base.Apply();
        }
    }
}
