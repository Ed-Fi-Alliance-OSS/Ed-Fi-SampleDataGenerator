using System.Collections.Generic;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public interface IBufferedEntityOutputService<in TEntity, in TConfiguration> : IEntityOutputService<TEntity, TConfiguration>
    {
        void FlushOutput();
    }

    public abstract class BufferedOutputService<TEntity, TConfiguration> : IBufferedEntityOutputService<TEntity, TConfiguration>
    {
        protected TConfiguration CurrentConfiguration { get; set; }
        protected List<TEntity> OutputBuffer { get; private set; }

        protected BufferedOutputService()
        {
            OutputBuffer = new List<TEntity>();
        } 

        public virtual void Configure(TConfiguration configuration)
        {
            FlushOutput();
            CurrentConfiguration = configuration;
        }

        public virtual void WriteToOutput(TEntity record)
        {
            OutputBuffer.Add(record);
        }

        protected abstract void WriteOutputToFile();

        public virtual void FlushOutput()
        {
            if (OutputBuffer.Count <= 0) return;

            WriteOutputToFile();
            OutputBuffer.Clear();
        }
    }
}