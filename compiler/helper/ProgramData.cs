using System;
using System.Linq;
using System.Collections.Generic;

using LL.AST;

namespace LL.Helper
{
    public class ProgramData
    {
        public List<ProgramNode> FilesInProgram { get; set; }
        public ProgramNode RootProgram { get; set; }

        public ProgramData()
        {
            this.FilesInProgram = new List<ProgramNode>();
        }

        /// <summary>
        /// Checks if the program contains circular dependencies by performing a <see href="https://en.wikipedia.org/wiki/Topological_sorting">topological sort</see>
        /// </summary>
        /// <returns>
        /// <para><see langword="true"/> if a circular dependency is detected,</para>
        /// <para><see langword="false"/> otherwise</para>
        /// </returns>
        /// <param name="nodes">
        /// If no circular dependency is detected, contains nodes in the order they need to be compiled.
        /// If a circular dependency is detected, contains nodes taking part in the circle
        /// </param>
        public bool ContainsCircularDependency(out List<ProgramNode> nodes)
        {
            nodes = new List<ProgramNode>();
            List<ProgramNode> hasNoDependants = new List<ProgramNode>();
            // expectation: the root program has no dependants
            hasNoDependants.Add(this.RootProgram);

            while (hasNoDependants.Count > 0)
            {
                ProgramNode prog = hasNoDependants.First();
                hasNoDependants.Remove(prog);
                this.FilesInProgram.Remove(prog);

                foreach (ProgramNode p in this.FilesInProgram)
                {
                    // if any other program depends on this one, the graph is not acyclic
                    if (p.Dependencies.Values.Any(load => load.Program == prog))
                    {
                        nodes = new List<ProgramNode>() { prog, p };
                        return true;
                    }
                }

                // the program has no more dependants
                nodes.Add(prog);

                foreach (LoadStatement load in prog.Dependencies.Values)
                {
                    // if dependency of program has still other dependencies -> skip
                    if (this.FilesInProgram.Any(p => p.Dependencies.ContainsValue(load)))
                        continue;

                    // has no other dependencies
                    hasNoDependants.Add(load.Program);
                }
            }

            // if there are still nodes in this set, there is a cycle in the graph which contains those nodes
            if (this.FilesInProgram.Count != 0)
            {
                nodes = this.FilesInProgram.ToList();
                return true;
            }

            // reverse the list to get the order of compilation
            nodes.Reverse();
            return false;
        }
    }
}